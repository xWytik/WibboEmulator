﻿using WibboEmulator.Communication.Packets.Outgoing.Inventory.Achievements;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Communication.Packets.Outgoing.Users;
using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;
using System.Data;
using WibboEmulator.Games.GameClients.Achievements;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;

namespace WibboEmulator.Games.Achievements
{
    public class AchievementManager
    {
        private readonly Dictionary<string, AchievementData> _achievements;

        public AchievementManager()
        {
            this._achievements = new Dictionary<string, AchievementData>();
        }

        public void Init(IQueryAdapter dbClient)
        {
            this._achievements.Clear();

            DataTable table = EmulatorAchievementDao.GetAll(dbClient);
            foreach (DataRow dataRow in table.Rows)
            {
                int Id = Convert.ToInt32(dataRow["id"]);
                string Category = (string)dataRow["category"];
                string GroupName = (string)dataRow["group_name"];

                if (!GroupName.StartsWith("ACH_"))
                {
                    GroupName = "ACH_" + GroupName;
                }

                AchievementLevel Level = new AchievementLevel(Convert.ToInt32(dataRow["level"]), Convert.ToInt32(dataRow["reward_pixels"]), Convert.ToInt32(dataRow["reward_points"]), Convert.ToInt32(dataRow["progress_needed"]));
                if (!this._achievements.ContainsKey(GroupName))
                {
                    AchievementData achievement = new AchievementData(Id, GroupName, Category);
                    achievement.AddLevel(Level);
                    this._achievements.Add(GroupName, achievement);
                }
                else
                {
                    this._achievements[GroupName].AddLevel(Level);
                }
            }
        }

        public void GetList(GameClient Session)
        {
            Session.SendPacket(new AchievementsComposer(Session, this._achievements.Values.ToList()));
        }

        public bool ProgressAchievement(GameClient Session, string AchievementGroup, int ProgressAmount)
        {
            if (!this._achievements.ContainsKey(AchievementGroup))
            {
                return false;
            }

            AchievementData AchievementData = this._achievements[AchievementGroup];

            UserAchievement userData = Session.GetUser().GetAchievementComponent().GetAchievementData(AchievementGroup);

            if (userData == null)
            {
                userData = new UserAchievement(AchievementGroup, 0, 0);
                Session.GetUser().GetAchievementComponent().AddAchievement(userData);
            }

            int TotalLevels = AchievementData.Levels.Count;

            if (userData != null && userData.Level == TotalLevels)
            {
                return false;
            }

            int TargetLevel = (userData != null ? userData.Level + 1 : 1);

            if (TargetLevel > TotalLevels)
            {
                TargetLevel = TotalLevels;
            }

            AchievementLevel TargetLevelData = AchievementData.Levels[TargetLevel];

            int NewProgress = (userData != null ? userData.Progress + ProgressAmount : ProgressAmount);
            int NewLevel = (userData != null ? userData.Level : 0);
            int NewTarget = NewLevel + 1;

            if (NewTarget > TotalLevels)
            {
                NewTarget = TotalLevels;
            }

            if (NewProgress >= TargetLevelData.Requirement)
            {
                NewLevel++;
                NewTarget++;

                int ProgressRemainder = NewProgress - TargetLevelData.Requirement;
                NewProgress = 0;

                Session.GetUser().GetBadgeComponent().GiveBadge(AchievementGroup + TargetLevel, true);
                Session.SendPacket(new ReceiveBadgeComposer(AchievementGroup + TargetLevel));

                if (NewTarget > TotalLevels)
                {
                    NewTarget = TotalLevels;
                }

                Session.GetUser().Duckets += TargetLevelData.RewardPixels;
                Session.SendPacket(new ActivityPointNotificationComposer(Session.GetUser().Duckets, 1));

                Session.SendPacket(new AchievementUnlockedComposer(AchievementData, TargetLevel, TargetLevelData.RewardPoints, TargetLevelData.RewardPixels));

                using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    UserAchievementDao.Replace(dbClient, Session.GetUser().Id, NewLevel, NewProgress, AchievementGroup);
                    UserStatsDao.UpdateAchievementScore(dbClient, Session.GetUser().Id, TargetLevelData.RewardPoints);
                }

                if (userData != null)
                {
                    userData.Level = NewLevel;
                    userData.Progress = NewProgress;
                }

                Session.GetUser().AchievementPoints += TargetLevelData.RewardPoints;
                Session.GetUser().Duckets += TargetLevelData.RewardPixels;
                Session.SendPacket(new ActivityPointNotificationComposer(Session.GetUser().Duckets, 1));
                Session.SendPacket(new AchievementScoreComposer(Session.GetUser().AchievementPoints));


                if (Session.GetUser().CurrentRoom != null)
                {
                    RoomUser roomUserByUserId = Session.GetUser().CurrentRoom.GetRoomUserManager().GetRoomUserByUserId(Session.GetUser().Id);
                    if (roomUserByUserId != null)
                    {
                        Session.SendPacket(new UserChangeComposer(roomUserByUserId, true));
                        Session.GetUser().CurrentRoom.SendPacket(new UserChangeComposer(roomUserByUserId, false));
                    }
                }


                AchievementLevel NewLevelData = AchievementData.Levels[NewTarget];
                Session.SendPacket(new AchievementProgressedComposer(AchievementData, NewTarget, NewLevelData, TotalLevels, Session.GetUser().GetAchievementComponent().GetAchievementData(AchievementGroup)));

                return true;
            }
            else
            {
                if (userData != null)
                {
                    userData.Level = NewLevel;
                    userData.Progress = NewProgress;
                }

                using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
                    UserAchievementDao.Replace(dbClient, Session.GetUser().Id, NewLevel, NewProgress, AchievementGroup);

                Session.SendPacket(new AchievementProgressedComposer(AchievementData, TargetLevel, TargetLevelData,
                TotalLevels, Session.GetUser().GetAchievementComponent().GetAchievementData(AchievementGroup)));
            }

            return false;
        }

        public AchievementData GetAchievement(string AchievementGroup)
        {
            if (this._achievements.ContainsKey(AchievementGroup))
            {
                return this._achievements[AchievementGroup];
            }

            return null;
        }
    }
}
