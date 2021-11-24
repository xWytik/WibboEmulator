using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Game.Clients;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class GuideEndSessionEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            Client requester = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().GuideOtherUserId);

            ServerPacket message = new ServerPacket(ServerPacketHeader.OnGuideSessionEnded);
            message.WriteInteger(1);
            Session.SendPacket(message);

            Session.GetHabbo().GuideOtherUserId = 0;
            if (Session.GetHabbo().OnDuty)
            {
                ButterflyEnvironment.GetGame().GetHelpManager().EndService(Session.GetHabbo().Id);
            }

            if (requester != null)
            {
                requester.SendPacket(message);
                requester.GetHabbo().GuideOtherUserId = 0;

                if (requester.GetHabbo().OnDuty)
                {
                    ButterflyEnvironment.GetGame().GetHelpManager().EndService(requester.GetHabbo().Id);
                }
            }
        }
    }
}