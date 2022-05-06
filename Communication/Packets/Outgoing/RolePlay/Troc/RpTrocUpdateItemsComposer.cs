﻿using Butterfly.Game.Roleplay;
using System.Collections.Generic;

namespace Butterfly.Communication.Packets.Outgoing.RolePlay.Troc
{
    internal class RpTrocUpdateItemsComposer : ServerPacket
    {
        public RpTrocUpdateItemsComposer(int UserId, Dictionary<int, int> Items)
          : base(ServerPacketHeader.RP_TROC_UPDATE_ITEMS)
        {
            this.WriteInteger(UserId);
            this.WriteInteger(Items.Count);

            foreach (KeyValuePair<int, int> Item in Items)
            {
                RPItem RpItem = ButterflyEnvironment.GetGame().GetRoleplayManager().GetItemManager().GetItem(Item.Key);

                this.WriteInteger(Item.Key);
                this.WriteString((RpItem == null) ? "" : RpItem.Name);
                this.WriteString((RpItem == null) ? "" : RpItem.Desc);
                this.WriteInteger(Item.Value);
            }
        }
    }
}