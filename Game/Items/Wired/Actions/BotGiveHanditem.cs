﻿using Wibbo.Database.Interfaces;
using Wibbo.Game.Rooms;
using Wibbo.Game.Items.Wired.Interfaces;
using System.Data;

namespace Wibbo.Game.Items.Wired.Actions
{
    public class BotGiveHanditem : WiredActionBase, IWired, IWiredEffect
    {
        public BotGiveHanditem(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_GIVE_HAND_ITEM)
        {
            this.IntParams.Add(0);
        }

        public override bool OnCycle(RoomUser user, Item item)
        {
            return false;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            int handItemId = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0);

            WiredUtillity.SaveTriggerItem(dbClient, this.Id, handItemId.ToString(), this.StringParam, false, null, this.Delay);
        }

        public void LoadFromDatabase(DataRow row)
        {
            this.IntParams.Clear();

            if (int.TryParse(row["delay"].ToString(), out int delay))
	            this.Delay = delay;
                
            this.StringParam = row["trigger_data"].ToString();

            if (int.TryParse(row["trigger_data_2"].ToString(), out int handItemId))
                this.IntParams.Add(handItemId);
        }
    }
}