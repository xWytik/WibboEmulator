﻿using Butterfly.Communication.Packets.Outgoing.WebSocket;
using Butterfly.Game.Clients;
using Butterfly.Game.Rooms;

namespace Butterfly.Game.Chat.Commands.Cmd
{
    internal class OpenWeb : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length != 3)
            {
                return;
            }

            Client clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (clientByUsername == null || clientByUsername.GetHabbo() == null)
            {
                return;
            }

            clientByUsername.GetHabbo().SendWebPacket(new NavigateWebComposer(Params[2]));
        }
    }
}