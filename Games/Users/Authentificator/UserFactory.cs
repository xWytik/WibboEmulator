﻿using WibboEmulator.Core;
using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using System.Data;

namespace WibboEmulator.Games.GameClients.Authenticator
{
    public class UserFactory
    {
        public static User GetUserData(string sessionTicket, string ip, string machineid)
        {
            try
            {
                int userId;
                DataRow dUserInfo;
                DataRow dUserStats;
                double ignoreAllExpire = 0;
                bool changeName = false;

                using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dUserInfo = UserDao.GetOneByTicket(dbClient, sessionTicket);
                    if (dUserInfo == null)
                    {
                        return null;
                    }

                    bool IsBanned = BanDao.IsBanned(dbClient, dUserInfo["username"].ToString(), ip, dUserInfo["ip_last"].ToString(), machineid);
                    if (IsBanned)
                    {
                        return null;
                    }

                    double IgnoreAll = BanDao.GetOneIgnoreAll(dbClient, dUserInfo["username"].ToString());
                    if (IgnoreAll > 0)
                    {
                        ignoreAllExpire = IgnoreAll;
                    }

                    userId = Convert.ToInt32(dUserInfo["id"]);
                    string username = (string)dUserInfo["username"];
                    if (WibboEnvironment.GetGame().GetClientManager().GetClientByUserID(userId) != null)
                    {
                        WibboEnvironment.GetGame().GetClientManager().GetClientByUserID(userId).Disconnect();
                        return null;
                    }

                    string lastDailyCredits = (string)dUserInfo["lastdailycredits"];
                    string lastDaily = DateTime.Today.ToString("MM/dd");
                    if (lastDailyCredits != lastDaily)
                    {
                        UserDao.UpdateLastDailyCredits(dbClient, userId, lastDaily);
                        dUserInfo["credits"] = (Convert.ToInt32(dUserInfo["credits"]) + 3000);

                        if (Convert.ToInt32(dUserInfo["rank"]) <= 1)
                        {
                            UserStatsDao.UpdateRespectPoint(dbClient, userId, 5);
                        }
                        else
                        {
                            UserStatsDao.UpdateRespectPoint(dbClient, userId, 20);
                        }

                        changeName = true;
                    }

                    if (!sessionTicket.StartsWith("monticket"))
                    {
                        UserDao.UpdateOnline(dbClient, userId);
                    }

                    dUserStats = UserStatsDao.GetOne(dbClient, userId);

                    if (dUserStats == null)
                    {
                        UserStatsDao.Insert(dbClient, userId);
                        dUserStats = UserStatsDao.GetOne(dbClient, userId);
                    }
                }

                return GenerateUser(dUserInfo, dUserStats, changeName, ignoreAllExpire);
            }
            catch (Exception ex)
            {
                ExceptionLogger.HandleException(ex, "UserDataFactory.GetUserData");            
                return null;
            }
        }

        public static User GetUserData(int userId)
        {
            DataRow dUserInfo;
            DataRow dUserStats;

            if (WibboEnvironment.GetGame().GetClientManager().GetClientByUserID(userId) != null)
            {
                return null;
            }

            using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dUserInfo = UserDao.GetOne(dbClient, userId);
                if (dUserInfo == null)
                {
                    return null;
                }

                dUserStats = UserStatsDao.GetOne(dbClient, userId);

                if (dUserStats == null)
                {
                    UserStatsDao.Insert(dbClient, userId);
                    dUserStats = UserStatsDao.GetOne(dbClient, userId);
                }
            }

            return GenerateUser(dUserInfo, dUserStats, false, 0);
        }

        public static User GenerateUser(DataRow dRow, DataRow dRow2, bool ChangeName, double ignoreAllExpire)
        {
            int Id = Convert.ToInt32(dRow["id"]);
            string Username = (string)dRow["username"];
            int Rank = Convert.ToInt32(dRow["rank"]);
            string Motto = (string)dRow["motto"];
            string Look = (string)dRow["look"];
            string Gender = (string)dRow["gender"];
            int LastOnline = Convert.ToInt32(dRow["last_online"]);
            int Credits = Convert.ToInt32(dRow["credits"]);
            int Diamonds = Convert.ToInt32(dRow["vip_points"]);
            int LimitCoins = Convert.ToInt32(dRow["limit_coins"]);
            int ActivityPoints = Convert.ToInt32(dRow["activity_points"]);
            int HomeRoom = Convert.ToInt32(dRow["home_room"]);
            int accountCreated = Convert.ToInt32(dRow["account_created"]);
            bool AcceptTrading = WibboEnvironment.EnumToBool(dRow["accept_trading"].ToString());
            string Ip = dRow["ip_last"].ToString();
            bool HideInroom = WibboEnvironment.EnumToBool(dRow["hide_inroom"].ToString());
            bool HideOnline = WibboEnvironment.EnumToBool(dRow["hide_online"].ToString());
            int MazoHighScore = Convert.ToInt32(dRow["mazoscore"]);
            int Mazo = Convert.ToInt32(dRow["mazo"]);
            string clientVolume = (string)dRow["volume"];
            bool NuxEnable = WibboEnvironment.EnumToBool(dRow["nux_enable"].ToString());
            bool IgnoreRoomInvite = WibboEnvironment.EnumToBool(dRow["ignore_room_invite"].ToString());
            bool CameraFollowDisabled = WibboEnvironment.EnumToBool(dRow["camera_follow_disabled"].ToString());
            string MachineId = (string)dRow["machine_id"];
            Language Langue = LanguageManager.ParseLanguage((string)dRow["langue"]);

            int Respect = Convert.ToInt32(dRow2["respect"]);
            int DailyRespectPoints = Convert.ToInt32(dRow2["daily_respect_points"]);
            int DailyPetRespectPoints = Convert.ToInt32(dRow2["daily_pet_respect_points"]);
            bool HasFriendRequestsDisabled = WibboEnvironment.EnumToBool(dRow["block_newfriends"].ToString());
            int currentQuestID = Convert.ToInt32(dRow2["quest_id"]);
            int achievementPoints = Convert.ToInt32(dRow2["achievement_score"]);
            int FavoriteGroup = Convert.ToInt32(dRow2["group_id"]);

            return new User(Id, Username, Rank, Motto, Look, Gender, Credits, Diamonds, LimitCoins, ActivityPoints, HomeRoom, Respect, DailyRespectPoints, DailyPetRespectPoints, HasFriendRequestsDisabled, currentQuestID, achievementPoints, LastOnline, FavoriteGroup, accountCreated, AcceptTrading, Ip, HideInroom, HideOnline, MazoHighScore, Mazo, clientVolume, NuxEnable, MachineId, ChangeName, Langue, ignoreAllExpire, IgnoreRoomInvite, CameraFollowDisabled);
        }
    }
}