using Butterfly.Game.GameClients;
            {
                return;
            }

            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                {
                    return;
                }

                clientByUsername.SendNotification(message);