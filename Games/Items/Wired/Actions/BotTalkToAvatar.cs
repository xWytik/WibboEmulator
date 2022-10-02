﻿
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.AI;
using WibboEmulator.Games.Items.Wired.Interfaces;
using System.Data;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Chat;
using WibboEmulator.Communication.Packets.Outgoing.RolePlay;

namespace WibboEmulator.Games.Items.Wired.Actions
{
    public class BotTalkToAvatar : WiredActionBase, IWired, IWiredEffect
    {
        public BotTalkToAvatar(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_TALK_DIRECT_TO_AVTR)
        {
            this.IntParams.Add(0);
        }

        public override bool OnCycle(RoomUser user, Item item)
        {
            if (!this.StringParam.Contains('\t'))
                return false;

            string[] splitData = this.StringParam.Split('\t');

            string name = splitData[0].ToString();
            string message = splitData[1].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(message) || user == null || user.GetClient() == null)
            {
                return false;
            }

            RoomUser bot = this.RoomInstance.GetRoomUserManager().GetBotOrPetByName(name);
            if (bot == null || bot.BotData == null)
            {
                return false;
            }

            bool isWhisper = (((this.IntParams.Count > 0) ? this.IntParams[0] : 0)) == 1;

            string textMessage = message;
            textMessage = textMessage.Replace("#username#", user.GetUsername());
            textMessage = textMessage.Replace("#point#", user.WiredPoints.ToString());
            textMessage = textMessage.Replace("#roomname#", this.RoomInstance.RoomData.Name.ToString());
            textMessage = textMessage.Replace("#vote_yes#", this.RoomInstance.VotedYesCount.ToString());
            textMessage = textMessage.Replace("#vote_no#", this.RoomInstance.VotedNoCount.ToString());
            textMessage = textMessage.Replace("#wpcount#", user.GetClient().GetUser() != null ? user.GetClient().GetUser().WibboPoints.ToString() : "0");

            if (user.Roleplayer != null)
            {
                textMessage = textMessage.Replace("#money#", user.Roleplayer.Money.ToString());
            }

            if (isWhisper && textMessage.Contains(" : ") && (this.RoomInstance.IsRoleplay || this.RoomInstance.RoomData.OwnerName == "LieuPublic"))
            {
                this.SendBotChoose(textMessage, user, bot.BotData);
            }

            if (isWhisper)
            {
                user.GetClient().SendPacket(new WhisperComposer(bot.VirtualId, textMessage, 2));
            }
            else
            {
                user.GetClient().SendPacket(new ChatComposer(bot.VirtualId, textMessage, 2));
            }

            return false;
        }

        private void SendBotChoose(string TextMessage, RoomUser user, RoomBot BotData)
        {
            string[] SplitText = TextMessage.Split(new[] { " : " }, StringSplitOptions.None);
            if (SplitText.Length != 2)
            {
                return;
            }

            List<string[]> ChooseList = new List<string[]>
            {
                new List<string>
                {
                    BotData.Name,
                    SplitText[0],
                    SplitText[1],
                    BotData.Look
                }.ToArray()
            };

            user.GetClient().SendPacket(new BotChooseComposer(ChooseList));
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            bool isWhisper = (((this.IntParams.Count > 0) ? this.IntParams[0] : 0)) == 1;

            WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, isWhisper, null, this.Delay);
        }

        public void LoadFromDatabase(DataRow row)
        {
            this.IntParams.Clear();

            if (int.TryParse(row["delay"].ToString(), out int delay))
	            this.Delay = delay;
                
            if (bool.TryParse(row["all_user_triggerable"].ToString(), out bool isWhisper))
                this.IntParams.Add(isWhisper ? 1 : 0);

            string Data = row["trigger_data"].ToString();

            if (string.IsNullOrWhiteSpace(Data) || !Data.Contains('\t'))
            {
                return;
            }

            this.StringParam = Data;
        }
    }
}