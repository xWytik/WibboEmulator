using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Game.GameClients;
using System.Linq;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
            {
                return;
            }

            List<ServerPacket> MessageList = new List<ServerPacket>();
                {
                    continue;
                }

                MessageList.Add(room.GetRoomItemHandler().TeleportUser(user, UserRoom.Coordinate, 0, room.GetGameMap().SqAbsoluteHeight(UserRoom.X, UserRoom.Y)));