using Butterfly.Communication.Packets.Outgoing.Rooms.Chat;

using Butterfly.Game.Clients;
using Butterfly.Game.Quests;
using Butterfly.Game.Roleplay.Player;
using Butterfly.Game.Rooms;
using Butterfly.Game.Chat.Styles;
using Butterfly.Utilities;
using System;
using System.Text.RegularExpressions;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class ChatEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            if (Room.IsRoleplay)
            {
                RolePlayer Rp = User.Roleplayer;
                if (Rp != null && Rp.Dead)
                {
                    return;
                }
            }

            string Message = Packet.PopString();

            if (Message.Length > 100)
            {
                Message = Message.Substring(0, 100);
            }

            int Colour = Packet.PopInt();

            if (!ButterflyEnvironment.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out ChatStyle Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().HasFuse(Style.RequiredRight)))
            {
                Colour = 0;
            }

            if (Colour != 23)
            {
                Message = StringCharFilter.Escape(Message);
            }

            User.Unidle();

            if (Session.GetHabbo().Rank < 5 && Room.RoomMuted && !User.IsOwner() && !Session.GetHabbo().CurrentRoom.CheckRights(Session))
            {
                User.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("room.muted", Session.Langue));
                return;
            }

            if (Room.GetJanken().PlayerStarted(User))
            {
                if (!Room.GetJanken().PickChoice(User, Message))
                {
                    User.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("janken.choice", Session.Langue));
                }

                return;
            }

            if (Room.UserIsMuted(Session.GetHabbo().Id))
            {
                if (!Room.HasMuteExpired(Session.GetHabbo().Id))
                {
                    User.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("user.muted", Session.Langue));
                    return;
                }
                else
                {
                    Room.RemoveMute(Session.GetHabbo().Id);
                }
            }

            TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().SpamFloodTime;
            if (timeSpan.TotalSeconds > Session.GetHabbo().SpamProtectionTime && Session.GetHabbo().SpamEnable)
            {
                User.FloodCount = 0;
                Session.GetHabbo().SpamEnable = false;
            }
            else if (timeSpan.TotalSeconds > 4.0)
            {
                User.FloodCount = 0;
            }

            if (timeSpan.TotalSeconds < Session.GetHabbo().SpamProtectionTime && Session.GetHabbo().SpamEnable)
            {
                int i = Session.GetHabbo().SpamProtectionTime - timeSpan.Seconds;
                User.GetClient().SendPacket(new FloodControlComposer(i));
                return;
            }
            else if (timeSpan.TotalSeconds < 4.0 && User.FloodCount > 5 && !Session.GetHabbo().HasFuse("fuse_mod"))
            {
                Session.GetHabbo().SpamProtectionTime = (Room.IsRoleplay || Session.GetHabbo().HasFuse("fuse_low_flood")) ? 5 : 30;
                Session.GetHabbo().SpamEnable = true;

                User.GetClient().SendPacket(new FloodControlComposer(Session.GetHabbo().SpamProtectionTime - timeSpan.Seconds));

                return;
            }
            else if (Message.Length > 40 && Message == User.LastMessage && User.LastMessageCount == 1)
            {
                User.LastMessageCount = 0;
                User.LastMessage = "";

                Session.GetHabbo().SpamProtectionTime = (Room.IsRoleplay || Session.GetHabbo().HasFuse("fuse_low_flood")) ? 5 : 30;
                Session.GetHabbo().SpamEnable = true;
                User.GetClient().SendPacket(new FloodControlComposer(Session.GetHabbo().SpamProtectionTime - timeSpan.Seconds));
                return;
            }
            else
            {
                if (Message == User.LastMessage && Message.Length > 40)
                {
                    User.LastMessageCount++;
                }

                User.LastMessage = Message;

                Session.GetHabbo().SpamFloodTime = DateTime.Now;
                User.FloodCount++;

                if (Message.StartsWith("@red@") || Message.StartsWith("@rouge@"))
                {
                    User.ChatTextColor = "@red@";
                }
                else if (Message.StartsWith("@cyan@"))
                {
                    User.ChatTextColor = "@cyan@";
                }
                else if (Message.StartsWith("@blue@") || Message.StartsWith("@bleu@"))
                {
                    User.ChatTextColor = "@blue@";
                }
                else if (Message.StartsWith("@green@") || Message.StartsWith("@vert@"))
                {
                    User.ChatTextColor = "@green@";
                }
                else if (Message.StartsWith("@purple@") || Message.StartsWith("@violet@"))
                {
                    User.ChatTextColor = "@purple@";
                }
                else if (Message.StartsWith("@black@") || Message.StartsWith("@noir@"))
                {
                    User.ChatTextColor = "";
                }

                if (Message.StartsWith(":", StringComparison.CurrentCulture) && ButterflyEnvironment.GetGame().GetChatManager().GetCommands().Parse(Session, User, Room, Message))
                {
                    Room.GetChatMessageManager().AddMessage(Session.GetHabbo().Id, Session.GetHabbo().Username, Room.Id, string.Format("{0} a utiliser la commande {1}", Session.GetHabbo().Username, Message), UnixTimestamp.GetNow());
                    return;
                }

                if (Session.Antipub(Message, "<TCHAT>", Room.Id))
                {
                    return;
                }

                ButterflyEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT, 0);
                Session.GetHabbo().GetChatMessageManager().AddMessage(Session.GetHabbo().Id, Session.GetHabbo().Username, Room.Id, Message, UnixTimestamp.GetNow());
                Room.GetChatMessageManager().AddMessage(Session.GetHabbo().Id, Session.GetHabbo().Username, Room.Id, Message, UnixTimestamp.GetNow());

                if (User.transfbot)
                {
                    Colour = 2;
                }
            }

            if (!Session.GetHabbo().HasFuse("word_filter_override"))
            {
                Message = ButterflyEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Message);
                Message = new Regex(@"\[tag\](.*?)\[\/tag\]").Replace(Message, "<tag>$1</tag>");
            }

            if (Room.AllowsShous(User, Message))
            {
                User.SendWhisperChat(Message, false);
                return;
            }

            Room.OnUserSay(User, Message, false);

            if (User.IsSpectator && Session.GetHabbo().Rank < 11)
            {
                return;
            }

            if (User.muted && !Session.GetHabbo().HasFuse("fuse_tool"))
            {
                User.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("user.muted", Session.Langue));
                return;
            }

            if (!Session.GetHabbo().IgnoreAll)
            {
                Message = ButterflyEnvironment.GetGame().GetChatManager().GetMention().Parse(Session, Message);
            }

            if (!string.IsNullOrEmpty(User.ChatTextColor))
            {
                Message = User.ChatTextColor + " " + Message;
            }

            User.OnChat(Message, Colour, false);
        }
    }
}
