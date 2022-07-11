﻿using WibboEmulator.Communication.Packets.Outgoing.Rooms.Chat;
using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class StaffAlert : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length < 2)
            {
                return;
            }

            string MessageTxt = CommandManager.MergeParams(Params, 1);

            if (string.IsNullOrEmpty(MessageTxt))
            {
                return;
            }

            foreach (Client Staff in WibboEnvironment.GetGame().GetClientManager().GetClients)
            {
                if (Staff == null)
                {
                    continue;
                }

                if (Staff.GetUser() == null)
                {
                    continue;
                }

                if (Staff.GetUser().CurrentRoom == null)
                {
                    continue;
                }

                if (Staff.GetUser().Rank < 3)
                {
                    continue;
                }

                RoomUser User = Staff.GetUser().CurrentRoom.GetRoomUserManager().GetRoomUserByUserId(Staff.GetUser().Id);

                User.GetClient().SendPacket(new WhisperComposer(User.VirtualId, "[STAFF ALERT] " + MessageTxt + " - " + UserRoom.GetUsername(), 23));
            }
        }
    }
}
