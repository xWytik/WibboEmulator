﻿using Butterfly.Game.GameClients;

namespace Butterfly.Game.Items.Interactors
{
    public class InteractorGate : FurniInteractor
    {
        public InteractorGate()
        {
        }

        public override void OnPlace(GameClient Session, Item Item)
        {
        }

        public override void OnRemove(GameClient Session, Item Item)
        {
        }

        public override void OnTrigger(GameClient Session, Item Item, int Request, bool UserHasRights)
        {

            if (!UserHasRights)
            {
                return;
            }

            int newMode = 0;

            int.TryParse(Item.ExtraData, out int currentMode);

            if (currentMode == 0)
            {
                newMode = 1;
            }
            else
            {
                newMode = 0;
            }

            Item.ExtraData = newMode.ToString();
            Item.UpdateState();
            Item.GetRoom().GetGameMap().updateMapForItem(Item);
        }
    }
}