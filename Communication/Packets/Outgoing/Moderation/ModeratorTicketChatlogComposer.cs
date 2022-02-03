using Butterfly.Game.Chat.Logs;
using Butterfly.Game.Moderation;
using Butterfly.Game.Rooms;
using System.Collections.Generic;

namespace Butterfly.Communication.Packets.Outgoing.Moderation
{
    internal class ModeratorTicketChatlogComposer : ServerPacket
    {
        public ModeratorTicketChatlogComposer(ModerationTicket ticket, RoomData roomData, List<ChatlogEntry> chatlogs)
            : base(ServerPacketHeader.CFH_CHATLOG)
        {
            WriteInteger(ticket.TicketId);
            WriteInteger(ticket.SenderId);
            WriteInteger(ticket.ReportedId);
            WriteInteger(roomData.Id);

            WriteBoolean(false);
            WriteInteger(roomData.Id);
            WriteString(roomData.Name);

            WriteShort(chatlogs.Count);
            foreach (ChatlogEntry chat in chatlogs)
            {
                if (chat != null)
                {
                    WriteString(chat.timeSpoken.Hour + ":" + chat.timeSpoken.Minute); //this.timeSpoken.Minute
                    WriteInteger(chat.userID); //this.timeSpoken.Minute
                    WriteString(chat.username);
                    WriteString(chat.message);
                    WriteBoolean(false); // Text is bold
                }
                else
                {
                    WriteString("0"); //this.timeSpoken.Minute
                    WriteInteger(0); //this.timeSpoken.Minute
                    WriteString("");
                    WriteString("");
                    WriteBoolean(false); // Text is bold
                }
            }
        }
    }
}
