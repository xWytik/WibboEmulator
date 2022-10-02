﻿using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;

namespace WibboEmulator.Communication.RCON.Commands.User
{
    internal class AddPhotoCommand : IRCONCommand
    {
        public bool TryExecute(string[] parameters)
        {
            if (parameters.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parameters[1], out int Userid))
            {
                return false;
            }

            if (Userid == 0)
            {
                return false;
            }

            GameClient Client = WibboEnvironment.GetGame().GetGameClientManager().GetClientByUserID(Userid);
            if (Client == null)
            {
                return false;
            }

            string PhotoId = parameters[2];

            int photoItemId = WibboEnvironment.GetSettings().GetData<int>("photo.item.id");
            if (!WibboEnvironment.GetGame().GetItemManager().GetItem(photoItemId, out ItemData ItemData))
            {
                return false;
            }

            int Time = WibboEnvironment.GetUnixTimestamp();
            string ExtraData = "{\"w\":\"" + "/photos/" + PhotoId + ".png" + "\", \"n\":\"" + Client.GetUser().Username + "\", \"s\":\"" + Client.GetUser().Id + "\", \"u\":\"" + "0" + "\", \"t\":\"" + Time + "000" + "\"}";

            Item Item = ItemFactory.CreateSingleItemNullable(ItemData, Client.GetUser(), ExtraData);
            Client.GetUser().GetInventoryComponent().TryAddItem(Item);

            using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                UserPhotoDao.Insert(dbClient, Client.GetUser().Id, PhotoId, Time);
            }

            Client.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.buyphoto.valide", Client.Langue));

            return true;
        }
    }
}
