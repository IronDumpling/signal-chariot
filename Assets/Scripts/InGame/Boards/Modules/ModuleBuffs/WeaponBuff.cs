using UnityEngine;

namespace InGame.Boards.Modules.ModuleBuffs
{
    [System.Serializable]
    public class WeaponBuff: ModuleBuff
    {
        public override ModuleBuffType type => ModuleBuffType.Weapon;
        
        #region Bouncing
        public int bouncingBuff = 0;
        #endregion

        #region Splitting
        public int splittingCountBuff = 0;
        public int splittingSizeBuff = 0;
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

        protected override void OnAdd(ModuleBuff other)
        {
            WeaponBuff otherBuff = (WeaponBuff)other;

            bouncingBuff += otherBuff.bouncingBuff;
            splittingCountBuff += otherBuff.splittingCountBuff;
            splittingSizeBuff += otherBuff.splittingSizeBuff;
            penetrationBuff += otherBuff.penetrationBuff;
            numShotsFlatBuff += otherBuff.numShotsFlatBuff;
            speedPercentageBuff += otherBuff.speedPercentageBuff;
            damagePercentageBuff += otherBuff.damagePercentageBuff;
            flatDamageBuff += otherBuff.flatDamageBuff;
            bulletSizePercentageBuff += otherBuff.bulletSizePercentageBuff;
            lifeTimePercentageBuff += otherBuff.lifeTimePercentageBuff;
            numBulletsPerShotFlatBuff += otherBuff.numBulletsPerShotFlatBuff;
        }

        protected override void OnMinus(ModuleBuff other)
        {
            WeaponBuff otherBuff = (WeaponBuff)other;

            bouncingBuff -= otherBuff.bouncingBuff;
            splittingCountBuff -= otherBuff.splittingCountBuff;
            splittingSizeBuff -= otherBuff.splittingSizeBuff;
            penetrationBuff -= otherBuff.penetrationBuff;
            numShotsFlatBuff -= otherBuff.numShotsFlatBuff;
            speedPercentageBuff -= otherBuff.speedPercentageBuff;
            damagePercentageBuff -= otherBuff.damagePercentageBuff;
            flatDamageBuff -= otherBuff.flatDamageBuff;
            bulletSizePercentageBuff -= otherBuff.bulletSizePercentageBuff;
            lifeTimePercentageBuff -= otherBuff.lifeTimePercentageBuff;
            numBulletsPerShotFlatBuff -= otherBuff.numBulletsPerShotFlatBuff;
        }

        public override void SetDefault()
        {
            bouncingBuff = 0;
            splittingCountBuff = 0;
            splittingSizeBuff = 0;
            penetrationBuff = 0;
            numShotsFlatBuff = 0;
            speedPercentageBuff = 0;
            damagePercentageBuff = 0;
            flatDamageBuff = 0;
            bulletSizePercentageBuff = 0;
            lifeTimePercentageBuff = 0;
            numBulletsPerShotFlatBuff = 0;
        }

        public override ModuleBuff CreateCopy()
        {
            return new WeaponBuff
            {
                bouncingBuff = bouncingBuff,
                splittingCountBuff = splittingCountBuff,
                splittingSizeBuff = splittingSizeBuff,
                penetrationBuff = penetrationBuff,
                numShotsFlatBuff = numShotsFlatBuff,
                speedPercentageBuff = speedPercentageBuff,
                damagePercentageBuff = damagePercentageBuff,
                flatDamageBuff = flatDamageBuff,
                bulletSizePercentageBuff = bulletSizePercentageBuff,
                lifeTimePercentageBuff = lifeTimePercentageBuff,
                numBulletsPerShotFlatBuff = numBulletsPerShotFlatBuff
            };
        }

        public override string ToString()
        {
            return $"bouncingBuff: {bouncingBuff}, splittingCountBuff: {splittingCountBuff}, splittingSizeBuff: {splittingSizeBuff}, " + 
                   $"penetrationBuff, {penetrationBuff}, " +
                   $"numShotsFlatBuff: {numShotsFlatBuff}, numBulletsPerShotFlatBuff: {numBulletsPerShotFlatBuff}," +
                   $"speedPercentageBuff: {speedPercentageBuff}, " +
                   $"damagePercentageBuff: {damagePercentageBuff}, flatDamageBuff: {flatDamageBuff}, " +
                   $"bulletSizePercentageBuff: {bulletSizePercentageBuff}, lifeTimePercentageBuff: {lifeTimePercentageBuff}";
        }

        public static WeaponBuff CreateBuff()
        {
            return new WeaponBuff();
        }
        
        public static WeaponBuff CreateBuff(int bouncingBuff, int splittingCountBuff, int splittingSizeBuff, int penetrationBuff, 
            int numShotsFlatBuff, int numBulletsPerShotFlatBuff, int speedPercentageBuff, int damagePercentageBuff, 
            int flatDamageBuff, int bulletSizePercentageBuff, int lifeTimePercentageBuff)
        {
            return new WeaponBuff
            {
                bouncingBuff = bouncingBuff,
                splittingCountBuff = splittingCountBuff,
                splittingSizeBuff = splittingSizeBuff,
                penetrationBuff = penetrationBuff,
                numShotsFlatBuff = numShotsFlatBuff,
                numBulletsPerShotFlatBuff = numBulletsPerShotFlatBuff,
                speedPercentageBuff = speedPercentageBuff,
                damagePercentageBuff = damagePercentageBuff,
                flatDamageBuff = flatDamageBuff,
                bulletSizePercentageBuff = bulletSizePercentageBuff,
                lifeTimePercentageBuff = lifeTimePercentageBuff
            };
        }
    }
}