﻿using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.HabboHotel.Items
{
    public class ItemFactory
    {
        public static Item CreateSingleItemNullable(ItemData Data, Habbo Habbo, string ExtraData, int LimitedNumber = 0, int LimitedStack = 0)
        {
            if (Data == null)
            {
                throw new InvalidOperationException("Data cannot be null.");
            }

            Item Item = new Item(0, 0, Data.Id, ExtraData, LimitedNumber, LimitedStack, 0, 0, 0, 0, "", null);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO items (base_item,user_id,extra_data) VALUES (@did,@uid,@extra_data)");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("extra_data", ExtraData);
                Item.Id = Convert.ToInt32(dbClient.InsertQuery());

                if (LimitedNumber > 0)
                {
                    ItemLimitedDao.Insert(dbClient, Item.Id, LimitedNumber, LimitedStack);
                }

                return Item;
            }
        }

        public static Item CreateSingleItem(ItemData Data, Habbo Habbo, string ExtraData, int ItemId, int LimitedNumber = 0, int LimitedStack = 0)
        {
            if (Data == null)
            {
                return null;
            }

            int InsertId = 0;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO items (id,base_item,user_id,extra_data) VALUES (@id, @did,@uid,@extra_data)");
                dbClient.AddParameter("id", ItemId);
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("extra_data", ExtraData);
                InsertId = Convert.ToInt32(dbClient.InsertQuery());

                if (LimitedNumber > 0 && InsertId > 0)
                {
                    ItemLimitedDao.Insert(dbClient, ItemId, LimitedNumber, LimitedStack);
                }
            }

            if (InsertId <= 0)
            {
                return null;
            }

            Item Item = new Item(ItemId, 0, Data.Id, ExtraData, LimitedNumber, LimitedStack, 0, 0, 0, 0, "", null);
            return Item;
        }

        public static List<Item> CreateMultipleItems(ItemData Data, Habbo Habbo, string ExtraData, int Amount)
        {
            if (Data == null) throw new InvalidOperationException("Data cannot be null.");

            List<Item> Items = new List<Item>();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                for (int i = 0; i < Amount; i++)
                {
                    dbClient.SetQuery("INSERT INTO items (base_item,user_id,extra_data) VALUES (@did,@uid,@flags);");
                    dbClient.AddParameter("did", Data.Id);
                    dbClient.AddParameter("uid", Habbo.Id);
                    dbClient.AddParameter("flags", ExtraData);

                    Item Item = new Item(Convert.ToInt32(dbClient.InsertQuery()), 0, Data.Id, ExtraData, 0, 0, 0, 0, 0, 0, "", null);

                    Items.Add(Item);
                }
            }
            return Items;
        }

        public static List<Item> CreateTeleporterItems(ItemData Data, Habbo Habbo)
        {
            List<Item> Items = new List<Item>();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO items (base_item,user_id,extra_data) VALUES(@did,@uid,@flags);");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("flags", "");

                int Item1Id = Convert.ToInt32(dbClient.InsertQuery());

                dbClient.SetQuery("INSERT INTO items (base_item,user_id,extra_data) VALUES(@did,@uid,@flags);");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("flags", Item1Id.ToString());

                int Item2Id = Convert.ToInt32(dbClient.InsertQuery());

                Item Item1 = new Item(Item1Id, 0, Data.Id, "", 0, 0, 0, 0, 0, 0, "", null);
                Item Item2 = new Item(Item2Id, 0, Data.Id, "", 0, 0, 0, 0, 0, 0, "", null);

                ItemTeleportDao.Insert(dbClient, Item1Id, Item2Id);
                ItemTeleportDao.Insert(dbClient, Item2Id, Item1Id);

                Items.Add(Item1);
                Items.Add(Item2);
            }
            return Items;
        }

        public static void CreateMoodlightData(Item Item)
        {
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                ItemMoodlightDao.Insert(dbClient, Item.Id);
            }
        }
    }
}
