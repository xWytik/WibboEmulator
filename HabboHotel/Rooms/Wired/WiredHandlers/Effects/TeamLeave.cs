﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Communication.Packets.Outgoing.GameCenter;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Games;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    public class TeamLeave : IWired, IWiredEffect
    {
        private readonly int itemID;

        public TeamLeave(int itemID)
        {
            this.itemID = itemID;
        }

        public void Handle(RoomUser user, Item TriggerItem)
        {
            if (user != null && !user.IsBot && user.GetClient() != null && user.Team != Team.none && user.Room != null)
            {
                TeamManager managerForBanzai = user.Room.GetTeamManager();
                if (managerForBanzai == null)
                {
                    return;
                }

                managerForBanzai.OnUserLeave(user);
                user.Room.GetGameManager().UpdateGatesTeamCounts();
                user.ApplyEffect(0);
                user.Team = Team.none;

                user.GetClient().SendPacket(new IsPlayingComposer(false));
            }
        }

        public void Dispose()
        {

        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.itemID, string.Empty, string.Empty, false, null);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
        }

        public void OnTrigger(GameClient Session, int SpriteId)
        {
            ServerPacket Message = new ServerPacket(ServerPacketHeader.WIRED_ACTION);
            Message.WriteBoolean(false);
            Message.WriteInteger(0);
            Message.WriteInteger(0);
            Message.WriteInteger(SpriteId);
            Message.WriteInteger(this.itemID);
            Message.WriteString("");
            Message.WriteInteger(0);
            Message.WriteInteger(0); //7
            Message.WriteInteger(10);
            Message.WriteInteger(0);
            Message.WriteInteger(0);

            Session.SendPacket(Message);
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.RunQuery("DELETE FROM wired_items WHERE trigger_id = '" + this.itemID + "'");
        }
    }
}
