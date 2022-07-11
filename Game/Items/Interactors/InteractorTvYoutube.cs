﻿
using WibboEmulator.Communication.Packets.Outgoing.Televisions;
using WibboEmulator.Game.Clients;

namespace WibboEmulator.Game.Items.Interactors
{
    public class InteractorTvYoutube : FurniInteractor
    {
        public override void OnPlace(Client Session, Item Item)
        {
        }

        public override void OnRemove(Client Session, Item Item)
        {
        }

        public override void OnTrigger(Client Session, Item Item, int Request, bool UserHasRights, bool Reverse)
        {
            if (Session == null || Session.GetUser() == null)
            {
                return;
            }

            Session.SendPacket(new YoutubeTvComposer((UserHasRights) ? Item.Id : 0, Item.ExtraData));
        }

        public override void OnTick(Item item)
        {
        }
    }
}
