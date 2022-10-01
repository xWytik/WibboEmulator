using WibboEmulator.Communication.Packets.Outgoing.Help;
using WibboEmulator.Games.Clients;

namespace WibboEmulator.Communication.Packets.Incoming.Guide
{
    internal class VisitRoomGuidesEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            Client requester = WibboEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetUser().GuideOtherUserId);
            if (requester == null)
            {
                return;
            }

            Session.SendPacket(new OnGuideSessionRequesterRoomComposer(requester.GetUser().CurrentRoomId));
        }
    }
}
