﻿using Butterfly.Communication.Packets.Outgoing.Campaign;
using Butterfly.Game.Clients;

namespace Butterfly.Communication.Packets.Incoming.Campaign
{
    internal class OpenCampaignCalendarDoorAsStaffEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            string campaignName = Packet.PopString();
            int campaingnId = Packet.PopInt();

            Session.SendPacket(new CampaignCalendarDoorOpenedComposer(true, "", "", ""));
        }
    }
}
