﻿using WibboEmulator.Games.GameClients;

namespace WibboEmulator.Games.Items.Interactors
{
    public class InteractorSpinningBottle : FurniInteractor
    {
        public override void OnPlace(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, false);
        }

        public override void OnRemove(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
        }

        public override void OnTrigger(GameClient Session, Item Item, int Request, bool UserHasRights, bool Reverse)
        {
            if (!(Item.ExtraData != "-1"))
            {
                return;
            }

            Item.ExtraData = "-1";
            Item.UpdateState(false, true);
            Item.ReqUpdate(3);
        }

        public override void OnTick(Item item)
        {
            item.ExtraData = WibboEnvironment.GetRandomNumber(0, 7).ToString();
            item.UpdateState();
        }
    }
}