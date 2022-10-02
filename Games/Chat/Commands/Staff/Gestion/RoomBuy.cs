﻿using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Session;
using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

namespace WibboEmulator.Games.Chat.Commands.Staff.Gestion
{
    internal class RoomBuy : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Room.RoomData.SellPrice == 0)
            {
                return;
            }

            if (Session.GetUser().WibboPoints - Room.RoomData.SellPrice <= 0)
            {
                return;
            }

            Session.GetUser().WibboPoints -= Room.RoomData.SellPrice;
            Session.SendPacket(new ActivityPointNotificationComposer(Session.GetUser().WibboPoints, 0, 105));

            GameClient ClientOwner = WibboEnvironment.GetGame().GetGameClientManager().GetClientByUserID(Room.RoomData.OwnerId);
            if (ClientOwner != null && ClientOwner.GetUser() != null)
            {
                ClientOwner.GetUser().WibboPoints += Room.RoomData.SellPrice;
                ClientOwner.SendPacket(new ActivityPointNotificationComposer(ClientOwner.GetUser().WibboPoints, 0, 105));
            }

            using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                UserDao.UpdateRemovePoints(dbClient, Session.GetUser().Id, Room.RoomData.SellPrice);
                UserDao.UpdateAddPoints(dbClient, Room.RoomData.OwnerId, Room.RoomData.SellPrice);

                RoomRightDao.Delete(dbClient, Room.Id);
                RoomDao.UpdateOwner(dbClient, Room.Id, Session.GetUser().Username);
                RoomDao.UpdatePrice(dbClient, Room.Id, 0);
            }

            Session.SendNotification(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("roombuy.sucess", Session.Langue), Room.RoomData.SellPrice));

            Room.RoomData.SellPrice = 0;

            List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();
            WibboEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);

            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                {
                    continue;
                }

                User.GetClient().SendPacket(new RoomForwardComposer(Room.Id));
            }
        }
    }
}
