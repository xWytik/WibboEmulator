﻿using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms;
using System;

namespace Butterfly.Communication.Packets.Incoming.Televisions
{
    internal class EditTvYoutubeEvent : IPacketEvent
    {
        public double Delay => 500;

        public void Parse(Client Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();
            string Url = Packet.PopString();

            if (Session == null || Session.GetUser() == null)
            {
                return;
            }

            Room room = Session.GetUser().CurrentRoom;
            if (room == null || !room.CheckRights(Session))
            {
                return;
            }

            Item item = room.GetRoomItemHandler().GetItem(ItemId);
            if (item == null || item.GetBaseItem().InteractionType != InteractionType.TVYOUTUBE)
            {
                return;
            }

            if (string.IsNullOrEmpty(Url) || (!Url.Contains("?v=") && !Url.Contains("youtu.be/"))) //https://youtu.be/_mNig3ZxYbM
            {

            item.ExtraData = VideoId;
            item.UpdateState();
        }
    }
}