﻿using WibboEmulator.Games.Roleplay;

namespace WibboEmulator.Communication.Packets.Outgoing.RolePlay
{
    internal class BuyItemsListComposer : ServerPacket
    {
        public BuyItemsListComposer(List<RPItem> ItemsBuy)
          : base(ServerPacketHeader.BUY_ITEMS_LIST)
        {
            this.WriteInteger(ItemsBuy.Count);

            foreach (RPItem Item in ItemsBuy)
            {
                this.WriteInteger(Item.Id);
                this.WriteString(Item.Name);
                this.WriteString(Item.Desc);
                this.WriteInteger(Item.Price);
            }
        }
    }
}