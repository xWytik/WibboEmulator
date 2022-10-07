namespace WibboEmulator.Communication.Packets.Incoming.Messenger;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Quests;

internal class RequestBuddyEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        var userName = packet.PopString();

        if (session.GetUser().GetMessenger() == null || !session.GetUser().GetMessenger().RequestBuddy(userName))
        {
            return;
        }

        WibboEnvironment.GetGame().GetQuestManager().ProgressUserQuest(session, QuestType.SOCIAL_FRIEND, 0);
    }
}
