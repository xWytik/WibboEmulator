using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.Game.GameClients;
using Butterfly.Game.Users;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
{
    internal class Mazo : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Session.GetHabbo() == null)
            {
                return;
            }

            if (Room.IsRoleplay)
            {
                return;
            }

            int Nombre = ButterflyEnvironment.GetRandomNumber(1, 3);
            Habbo Habbo = Session.GetHabbo();

            if (Nombre != 1) //Gagné bravo +1Point
            {
                Habbo.Mazo += 1;

                if (Habbo.MazoHighScore < Habbo.Mazo)
                {
                    //SQL sauvegarde le score
                    UserRoom.SendWhisperChat(string.Format(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.mazo.newscore", Session.Langue), Habbo.Mazo));
                    Habbo.MazoHighScore = Habbo.Mazo;

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        UserDao.UpdateMazoScore(dbClient, Habbo.Id, Habbo.MazoHighScore);
                    }
                }
                else
                {
                    UserRoom.SendWhisperChat(string.Format(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.mazo.win", Session.Langue), Habbo.Mazo));
                }

                UserRoom.ApplyEffect(566, true);
                UserRoom.TimerResetEffect = 4;

            }
            else //Perdu remise à zero
            {
                //Mettre l'enable
                if (Habbo.Mazo > 0)
                {
                    UserRoom.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.mazo.bigloose", Session.Langue));
                }
                else
                {
                    UserRoom.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.mazo.loose", Session.Langue));
                }

                Habbo.Mazo = 0;

                UserRoom.ApplyEffect(567, true);
                UserRoom.TimerResetEffect = 4;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                UserDao.UpdateMazo(dbClient, Habbo.Id, Habbo.Mazo);
            }
        }
    }
}