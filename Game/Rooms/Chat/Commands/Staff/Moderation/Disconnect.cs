using Butterfly.Game.GameClients;
namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
            {
                Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("input.usernotfound", Session.Langue));
            }
            else if (clientByUsername.GetHabbo().Rank >= Session.GetHabbo().Rank)
                clientByUsername.Disconnect();








                /*WebClient ClientWeb = ButterflyEnvironment.GetGame().GetClientWebManager().GetClientByUserID(clientByUsername.GetHabbo().Id);
            }