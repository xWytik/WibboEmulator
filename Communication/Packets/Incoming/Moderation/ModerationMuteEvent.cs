namespace WibboEmulator.Communication.Packets.Incoming.Moderation;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Moderation;

internal class ModerationMuteEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetUser().HasPermission("perm_no_kick"))
        {
            return;
        }

        ModerationManager.KickUser(session, packet.PopInt(), packet.PopString(), false);
    }
}
