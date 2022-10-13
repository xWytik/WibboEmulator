namespace WibboEmulator.Communication.Packets.Incoming.Rooms.Avatar;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Avatar;

using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Quests;

internal class ActionEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetUser().CurrentRoomId, out var room))
        {
            return;
        }

        var roomUserByUserId = room.RoomUserManager.GetRoomUserByUserId(session.GetUser().Id);
        if (roomUserByUserId == null)
        {
            return;
        }

        roomUserByUserId.Unidle();
        var i = packet.PopInt();
        roomUserByUserId.DanceId = 0;

        room.SendPacket(new ActionComposer(roomUserByUserId.VirtualId, i));

        if (i == 5)
        {
            roomUserByUserId.IsAsleep = true;
            room.SendPacket(new SleepComposer(roomUserByUserId.VirtualId, true));
        }

        WibboEnvironment.GetGame().GetQuestManager().ProgressUserQuest(session, QuestType.SocialWave, 0);
    }
}
