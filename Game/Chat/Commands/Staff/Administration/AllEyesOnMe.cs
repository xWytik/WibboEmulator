﻿using Butterfly.Game.Clients;
using Butterfly.Game.Rooms.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using Butterfly.Game.Rooms;

namespace Butterfly.Game.Chat.Commands.Cmd
{
    internal class AllEyesOnMe : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser U in Users.ToList())
            {
                if (U == null || Session.GetHabbo().Id == U.UserId)
                    continue;

                U.SetRot(Rotation.Calculate(U.X, U.Y, ThisUser.X, ThisUser.Y), false);
            }
            UserRoom.SendWhisperChat("Tout les joueurs te regardent");
        }
    }
}