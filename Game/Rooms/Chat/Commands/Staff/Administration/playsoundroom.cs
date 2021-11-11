﻿using Butterfly.Communication.Packets.Outgoing.WebSocket;
using Butterfly.Game.GameClients;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
{
    internal class PlaySoundRoom : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length != 2)
            {
                return;
            }

            string SongName = Params[1];

            Room.SendPacketWeb(new PlaySoundComposer(SongName, 1)); //Type = Trax
        }
    }
}