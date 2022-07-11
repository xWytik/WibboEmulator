using WibboEmulator.Communication.Packets.Outgoing.Help;
using WibboEmulator.Game.Clients;

namespace WibboEmulator.Communication.Packets.Incoming.Structure
{
    internal class OpenHelpToolEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session.GetUser() == null || Session.GetUser().HasFuse("fuse_helptool"))
                return;

            Session.SendPacket(new OpenHelpToolComposer(0));
        }
    }
}
