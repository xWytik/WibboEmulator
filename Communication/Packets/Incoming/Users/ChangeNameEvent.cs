namespace WibboEmulator.Communication.Packets.Incoming.Users;
using WibboEmulator.Communication.Packets.Outgoing.Handshake;
using WibboEmulator.Communication.Packets.Outgoing.Navigator;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Communication.Packets.Outgoing.Users;
using WibboEmulator.Database.Daos.Log;
using WibboEmulator.Database.Daos.Room;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;

internal class ChangeNameEvent : IPacketEvent
{
    public double Delay => 5000;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session.GetUser() == null || session == null)
        {
            return;
        }

        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetUser().CurrentRoomId, out var room))
        {
            return;
        }

        var roomUser = room.GetRoomUserManager().GetRoomUserByName(session.GetUser().Username);
        if (roomUser == null)
        {
            return;
        }

        var newUsername = packet.PopString();

        if (!session.GetUser().CanChangeName && session.GetUser().Rank == 1)
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.changename.error.1", session.Langue));
            return;
        }

        if (newUsername == session.GetUser().Username)
        {
            session.SendPacket(new UpdateUsernameComposer(session.GetUser().Username));
            return;
        }

        if (NameAvailable(newUsername) != 1)
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.changename.error.2", session.Langue));
            return;
        }

        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            RoomDao.UpdateOwner(dbClient, newUsername, session.GetUser().Username);

            UserDao.UpdateName(dbClient, session.GetUser().Id, newUsername);

            LogFlagmeDao.Insert(dbClient, session.GetUser().Id, session.GetUser().Username, newUsername);
        }

        _ = WibboEnvironment.GetGame().GetGameClientManager().UpdateClientUsername(session.ConnectionID, session.GetUser().Username, newUsername);
        _ = room.GetRoomUserManager().UpdateClientUsername(roomUser, session.GetUser().Username, newUsername);
        session.GetUser().Username = newUsername;
        session.GetUser().CanChangeName = false;

        session.SendPacket(new UpdateUsernameComposer(newUsername));
        session.SendPacket(new UserObjectComposer(session.GetUser()));

        foreach (var roomId in session.GetUser().UsersRooms)
        {
            if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(roomId, out var roomOwner))
            {
                continue;
            }

            roomOwner.RoomData.OwnerName = newUsername;

            WibboEnvironment.GetGame().GetRoomManager().RoomDataRemove(roomId);
        }

        room.SendPacket(new UserNameChangeComposer(newUsername, roomUser.VirtualId));

        if (session.GetUser().Id == room.RoomData.OwnerId)
        {
            room.RoomData.OwnerName = newUsername;
            room.SendPacket(new RoomInfoUpdatedComposer(room.Id));
        }
    }

    private static int NameAvailable(string username)
    {
        username = username.ToLower();

        if (username.Length > 15)
        {
            return -2;
        }

        if (username.Length < 3)
        {
            return -2;
        }

        if (!WibboEnvironment.IsValidAlphaNumeric(username))
        {
            return -1;
        }

        return WibboEnvironment.UsernameExists(username) ? 0 : 1;
    }
}
