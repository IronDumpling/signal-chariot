using InGame.Boards.Modules.ModuleBuffs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editors.ModuleBuffs
{
    public class WeaponModuleBuffEdit: BuffElementEdit
    {
        #region Bouncing
        public int bouncingBuff = 0;
        #endregion
        
        #region Splitting
        public int splittingCountBuff = 0;
        [Min(1)] public int splittingSizeBuff = 1;
        #endregion
        
        #region Penetration
        public int penetrationBuff = 0; 
        #endregion
        
        #region Bullet Count
        public int numShotsFlatBuff = 0;
        public int numBulletsPerShotFlatBuff = 0;
        #endregion
        
        #region Damage
        public int damagePercentageBuff = 0;
        public int flatDamageBuff = 0;
        #endregion
        
        #region Speed
        public int speedPercentageBuff = 0; 
        #endregion
        
        #region Size
        public int bulletSizePercentageBuff = 0;
        #endregion

        #region Life Time
        public int lifeTimePercentageBuff = 0;
        #endregion
        
        public override ModuleBuff CreateBuff()
        {
            return WeaponBuff.CreateBuff(bouncingBuff, splittingCountBuff, splittingSizeBuff, 
                                        penetrationBuff, numShotsFlatBuff, numBulletsPerShotFlatBuff, 
                                        speedPercentageBuff, damagePercentageBuff, flatDamageBuff, 
                                        bulletSizePercentageBuff, lifeTimePercentageBuff);
        }
    }
}