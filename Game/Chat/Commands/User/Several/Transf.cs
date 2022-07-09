using Wibbo.Communication.Packets.Outgoing.Rooms.Engine;
using Wibbo.Game.Rooms;
using Wibbo.Game.Clients;
using Wibbo.Game.Rooms.Games;

namespace Wibbo.Game.Chat.Commands.Cmd
{
    internal class Transf : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (UserRoom.Team != TeamType.NONE || UserRoom.InGame)
            {
                return;
            }

            if (Session.GetUser().SpectatorMode || UserRoom.InGame)
            {
                return;
            }

            if (Params.Length == 3 || Params.Length == 2)
            {
                if (Room != null)
                {
                    int raceid = 0;
                    if (Params.Length == 3)
                    {
                        string x = Params[2];
                        if (int.TryParse(x, out int value))
                        {
                            raceid = Convert.ToInt32(Params[2]);
                            if (raceid < 1 || raceid > 50)
                            {
                                raceid = 0;
                            }
                        }
                    }
                    else
                    {
                        raceid = 0;
                    }

                    if (!UserRoom.SetPetTransformation(Params[1], raceid))
                    {
                        Session.SendHugeNotif(WibboEnvironment.GetLanguageManager().TryGetValue("cmd.transf.help", Session.Langue));
                        return;
                    }

                    UserRoom.IsTransf = true;

                    Room.SendPacket(new UserRemoveComposer(UserRoom.VirtualId));
                    Room.SendPacket(new UsersComposer(UserRoom));
                }
            }
            else
            {
                Session.SendHugeNotif(WibboEnvironment.GetLanguageManager().TryGetValue("cmd.transf.help", Session.Langue));
            }

        }
    }
}
