﻿using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Items.Wired.Interfaces;
using System.Data;
using System.Drawing;

namespace WibboEmulator.Games.Items.Wired.Conditions
{
    public class TriggerUserIsOnFurniNegative : WiredConditionBase, IWiredCondition, IWired
    {
        public TriggerUserIsOnFurniNegative(Item item, Room room) : base(item, room, (int)WiredConditionType.NOT_ACTOR_ON_FURNI)
        {
        }

        public bool AllowsExecution(RoomUser user, Item TriggerItem)
        {
            if (user == null)
            {
                return false;
            }

            foreach (Item roomItem in this.Items.ToList())
            {
                foreach (Point coord in roomItem.GetCoords)
                {
                    if (coord == user.Coordinate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, string.Empty, false, this.Items);
        }

        public void LoadFromDatabase(DataRow row)
        {
            string triggerItems = row["triggers_item"].ToString();

            if (triggerItems == null || triggerItems == "")
            {
                return;
            }

            foreach (string itemId in triggerItems.Split(';'))
            {
                if (!int.TryParse(itemId, out int id))
                    continue;

                if(!this.StuffIds.Contains(id))
                    this.StuffIds.Add(id);
            }
        }
    }
}
