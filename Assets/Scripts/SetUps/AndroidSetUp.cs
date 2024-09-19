using System;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;

namespace SetUps
{   
    [Serializable]
    public class AndroidSetUp
    {   
        #region Health
        public float initialHealth = 50;
        public float maxHealth = 100;
        #endregion

        #region Shield
        public float defense = 0;
        public int armor = 0;
        public int maxArmor = 1;
        #endregion

        #region Action
        public float speed = 1f;
        #endregion

        #region Resources
        public int initialMod = 0;
        public int maxMod = 500;
        public int initialCrystal = 0;
        public int maxCrystal = 50;
        #endregion

        public AndroidSetUp(Android android)
        {
            initialHealth = android.Get(LimitedPropertyType.Health, true);
            maxHealth = android.Get(LimitedPropertyType.Health, false);
            
            initialMod = (int)android.Get(LimitedPropertyType.Mod, true);
            maxMod = (int)android.Get(LimitedPropertyType.Mod, false);
            
            initialCrystal = (int)android.Get(LimitedPropertyType.Crystal, true);
            maxCrystal = (int)android.Get(LimitedPropertyType.Crystal, false);
            
            armor = (int)android.Get(LimitedPropertyType.Armor, true);
            maxArmor = (int)android.Get(LimitedPropertyType.Armor, false);
            
            defense = android.Get(UnlimitedPropertyType.Defense);
            

            speed = android.Get(UnlimitedPropertyType.Speed);
        }

        public void SetAndroid(Android android)
        {
            android.Set(LimitedPropertyType.Health, maxHealth, false);
            android.Set(LimitedPropertyType.Health, initialHealth, true);

            android.Set(LimitedPropertyType.Mod, maxMod, false);
            android.Set(LimitedPropertyType.Mod, initialMod, true);

            android.Set(LimitedPropertyType.Crystal, maxCrystal, false);
            android.Set(LimitedPropertyType.Crystal, initialCrystal, true);
            
            android.Set(LimitedPropertyType.Armor, maxArmor, false);
            android.Set(LimitedPropertyType.Armor, armor, true);
            
            android.Set(UnlimitedPropertyType.Defense, defense);
            

            android.Set(UnlimitedPropertyType.Speed, speed);
        }
    }
}