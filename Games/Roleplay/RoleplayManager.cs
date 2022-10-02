﻿using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Roleplay.Enemy;
using WibboEmulator.Games.Roleplay.Troc;
using WibboEmulator.Games.Roleplay.Weapon;
using System.Collections.Concurrent;
using System.Data;

namespace WibboEmulator.Games.Roleplay
{
    public class RoleplayManager
    {
        private readonly ConcurrentDictionary<int, RolePlayerManager> _rolePlay;
        private readonly RPItemManager _roleplayItemManager;
        private readonly RPWeaponManager _roleplayWeaponManager;
        private readonly RPEnemyManager _roleplayEnemyManager;
        private readonly RPTrocManager _roleplayTrocManager;

        public RoleplayManager()
        {
            this._rolePlay = new ConcurrentDictionary<int, RolePlayerManager>();

            this._roleplayItemManager = new RPItemManager();
            this._roleplayWeaponManager = new RPWeaponManager();
            this._roleplayEnemyManager = new RPEnemyManager();
            this._roleplayTrocManager = new RPTrocManager();
        }

        public RolePlayerManager GetRolePlay(int Ownerid)
        {
            if (!this._rolePlay.ContainsKey(Ownerid))
            {
                return null;
            }

            this._rolePlay.TryGetValue(Ownerid, out RolePlayerManager RP);
            return RP;
        }

        public RPTrocManager GetTrocManager()
        {
            return this._roleplayTrocManager;
        }

        public RPWeaponManager GetWeaponManager()
        {
            return this._roleplayWeaponManager;
        }

        public RPItemManager GetItemManager()
        {
            return this._roleplayItemManager;
        }

        public RPEnemyManager GetEnemyManager()
        {
            return this._roleplayEnemyManager;
        }

        public void Init(IQueryAdapter dbClient)
        {
            this._roleplayItemManager.Init(dbClient);
            this._roleplayWeaponManager.Init(dbClient);
            this._roleplayEnemyManager.Init(dbClient);

            DataTable table = RoleplayDao.GetAll(dbClient);
            if (table != null)
            {
                foreach (DataRow dataRow in table.Rows)
                {
                    if (!this._rolePlay.ContainsKey(Convert.ToInt32(dataRow["owner_id"])))
                    {
                        this._rolePlay.TryAdd(Convert.ToInt32(dataRow["owner_id"]), new RolePlayerManager(Convert.ToInt32(dataRow["owner_id"]), Convert.ToInt32(dataRow["hopital_id"]), Convert.ToInt32(dataRow["prison_id"])));
                    }
                    else
                    {
                        this.GetRolePlay(Convert.ToInt32(dataRow["owner_id"])).Update(Convert.ToInt32(dataRow["hopital_id"]), Convert.ToInt32(dataRow["prison_id"]));
                    }
                }
            }
        }
    }
}