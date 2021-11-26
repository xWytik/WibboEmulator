using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using System.Collections.Generic;
using System.Linq;
using Butterfly.Game.Rooms;

namespace Butterfly.Game.Chat.Commands.Cmd
{
    internal class CloseDice : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            List<Item> userBooth = Room.GetRoomItemHandler().GetFloor.Where(x => x != null && Gamemap.TilesTouching(
                x.X, x.Y, UserRoom.X, UserRoom.Y) && x.Data.InteractionType == InteractionType.DICE).ToList();

            if (userBooth == null)
            {
                return;
            }

            userBooth.ForEach(x =>
            {
                x.ExtraData = "0";
                x.UpdateState();
            });
        }
    }
}