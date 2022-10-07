namespace WibboEmulator.Games.Chat.Commands.Staff.Administration;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Avatar;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class RoomDance : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        if (parameters.Length == 1)
        {
            session.SendWhisper("Entrer une dance ID (1-4)");
            return;
        }

        var danceId = Convert.ToInt32(parameters[1]);
        if (danceId is < 0 or > 4)
        {
            session.SendWhisper("Entrer une dance ID (1-4)");
            return;
        }

        var users = room.GetRoomUserManager().GetRoomUsers();
        if (users.Count > 0)
        {
            foreach (var user in users.ToList())
            {
                if (user == null)
                {
                    continue;
                }

                if (user.CarryItemID > 0)
                {
                    user.CarryItemID = 0;
                }

                user.DanceId = danceId;
                room.SendPacket(new DanceComposer(user.VirtualId, danceId));
            }
        }
    }
}
