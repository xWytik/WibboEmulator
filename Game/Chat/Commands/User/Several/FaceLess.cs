using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Game.Rooms;
using WibboEmulator.Game.Clients;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class FaceLess : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (UserRoom.IsTransf || UserRoom.IsSpectator)
            {
                return;
            }

            string look = Session.GetUser().Look;

            if (look.Contains("hd-"))
            {
                string hdlook = look.Split(new string[] { "hd-" }, StringSplitOptions.None)[1];
                string hdcode = "hd-" + hdlook.Split(new char[] { '.' })[0]; //ex : hd-180-22
                string hdcodecolor = "";
                if (hdcode.Split('-').Length == 3)
                {
                    hdcodecolor = hdcode.Split('-')[2];
                }

                string hdcodenoface = "hd-99999-" + hdcodecolor; //hd-9999-22

                look = look.Replace(hdcode, hdcodenoface);

                Session.GetUser().Look = look;

                if (!Session.GetUser().InRoom)
                {
                    return;
                }

                Room currentRoom = Session.GetUser().CurrentRoom;
                if (currentRoom == null)
                {
                    return;
                }

                currentRoom.SendPacket(new UserChangeComposer(UserRoom, false));

            }

        }
    }
}
