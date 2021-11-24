using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Game.Clients;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class RecomendHelpersEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            Session.SendPacket(new ServerPacket(ServerPacketHeader.OnGuideSessionDetached));
        }
    }
}