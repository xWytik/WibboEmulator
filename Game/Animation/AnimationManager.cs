﻿
using Wibbo.Communication.Packets.Outgoing.Notifications.NotifCustom;
using Wibbo.Database.Daos;
using Wibbo.Database.Interfaces;
using Wibbo.Game.Rooms;
using System.Data;
using System.Diagnostics;

namespace Wibbo.Game.Animation
{
    public class AnimationManager
    {
        private const int MIN_USERS = 70;
        private const int START_TIME = 20;
        private const int NOTIF_TIME = 2;
        private const int CLOSE_TIME = 1;

        private List<int> _roomId;
        private bool _started;
        private bool _skipCycle;
        private int _timer;
        private int _roomIdGame;

        private bool _isActivate;
        private bool _notif;
        private bool _forceDisabled;
        private int _RoomIdIndex;

        public void OnUpdateUsersOnline(int usersOnline)
        {
            if (this._isActivate && usersOnline < MIN_USERS)
            {
                this._isActivate = false;
            }
            else if (!this._isActivate && usersOnline >= MIN_USERS)
            {
                this._isActivate = true;
            }
        }

        public bool ToggleForceDisabled()
        {
            this._forceDisabled = !this._forceDisabled;

            return this._forceDisabled;
        }

        public void ForceDisabled(bool Flag)
        {
            this._forceDisabled = Flag;
        }

        public AnimationManager()
        {
            this._roomId = new List<int>();
            this._started = false;
            this._timer = 0;
            this._roomIdGame = 0;
            this._isActivate = true;
            this._notif = false;
            this._skipCycle = false;
            this._forceDisabled = false;
        }

        public bool IsActivate()
        {
            return !this._forceDisabled && this._isActivate;
        }

        public bool AllowAnimation()
        {
            if (!this.IsActivate())
            {
                return true;
            }

            if (this._started)
            {
                return false;
            }

            if (this._timer >= this.ToSeconds(START_TIME - NOTIF_TIME))
            {
                return false;
            }

            this._timer = 0;

            return true;
        }

        public string GetTime()
        {
            TimeSpan time = TimeSpan.FromSeconds(this.ToSeconds(START_TIME) - this._timer);

            return time.Minutes + " minutes et " + time.Seconds + " secondes";
        }

        public void Init(IQueryAdapter dbClient)
        {
            this._roomId.Clear();

            DataTable table = RoomDao.GetAllByOwnerWibboGame(dbClient);
            if (table == null)
            {
                return;
            }

            foreach (DataRow dataRow in table.Rows)
            {
                if (this._roomId.Contains(Convert.ToInt32(dataRow["id"])))
                {
                    continue;
                }

                this._roomId.Add(Convert.ToInt32(dataRow["id"]));
            }

            if (this._roomId.Count == 0)
            {
                this._forceDisabled = true;
            }
        }

        private void Cycle()
        {
            if (!this._isActivate && !this._started)
            {
                return;
            }

            if (this._forceDisabled && !this._started)
            {
                return;
            }

            if (this._skipCycle)
            {
                this._timer++;
                this._skipCycle = false;
            }
            else
            {
                this._skipCycle = true;
            }

            if (this._started)
            {
                if (this._timer >= this.ToSeconds(CLOSE_TIME))
                {
                    Room room = WibboEnvironment.GetGame().GetRoomManager().LoadRoom(this._roomIdGame);

                    this._started = false;

                    if (room != null)
                    {
                        room.RoomData.State = 1;
                    }
                }
                return;
            }

            if (this._timer >= this.ToSeconds(START_TIME - NOTIF_TIME) && !this._notif)
            {
                this._notif = true;
                WibboEnvironment.GetGame().GetClientManager().SendMessage(new NotifTopComposer("Notre prochaine animation aura lieu dans deux minutes ! (Jack & Daisy)"));
            }

            if (this._timer >= this.ToSeconds(START_TIME))
            {
                this.StartGame();
            }
        }

        public void StartGame()
        {
            if (this._RoomIdIndex >= this._roomId.Count)
            {
                this._RoomIdIndex = 0;
                this._roomId = this._roomId.OrderBy(a => Guid.NewGuid()).ToList();
            }

            int RoomId = this._roomId[this._RoomIdIndex]; //ButterflyEnvironment.GetRandomNumber(0, this._roomId.Count - 1)
            this._RoomIdIndex++;

            Room room = WibboEnvironment.GetGame().GetRoomManager().LoadRoom(RoomId);
            if (room == null)
            {
                return;
            }

            this._timer = 0;
            this._started = true;
            this._notif = false;
            this._roomIdGame = RoomId;

            room.RoomData.State = 0;
            room.CloseFullRoom = true;

            string alertMessage = "[center] [size=large]🤖[b]Animation Jack & Daisy[/b] 🤖[/size][/center]" +
                "[br]" +
                "[center][i]Beep beep, c'est l'heure d'une animation automatisée ![/i][/center]" +
                "[br]" +
                "➤ Rejoins-nous chez [b]WibboGame[/b] pour un jeu qui s'intitule :: [b]" + room.RoomData.Name + "[/b] ::" +
                "[br][br]" +
                "➤ Rends-toi dans l'appartement et tente de remporter un lot composé de [i]une ou plusieurs RareBox(s) et BadgeBox(s) ainsi qu'un point au TOP Gamer ! [/i] 🎁" +
                "[br][br]" +
                "- Jack et Daisy";

            WibboEnvironment.GetGame().GetModerationManager().LogStaffEntry(1953042, "WibboGame", room.Id, string.Empty, "eventha", string.Format("JeuAuto EventHa: {0}", alertMessage));

            WibboEnvironment.GetGame().GetClientManager().SendMessage(new NotifAlertComposer(
                "gameauto", // image
                "Message d'animation", // title
                alertMessage, // string_>alert
                "Je veux y jouer !", // button
                room.Id, //guide
                ""
            ));
        }

        public void OnCycle(Stopwatch moduleWatch)
        {
            this.Cycle();
            this.HandleFunctionReset(moduleWatch, "AnimationCycle");
        }

        private int ToSeconds(int minutes)
        {
            return (minutes * 60);
        }

        private void HandleFunctionReset(Stopwatch watch, string methodName)
        {
            try
            {
                if (watch.ElapsedMilliseconds > 500)
                {
                    Console.WriteLine("High latency in {0}: {1}ms", methodName, watch.ElapsedMilliseconds);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Canceled operation {0}", e);

            }
            watch.Restart();
        }

    }
}
