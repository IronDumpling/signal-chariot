using System;

using UnityEngine;

using InGame.BattleFields.Androids;

namespace SetUps
{
    [Serializable]
    public class TowerSetUp
    {
        public Sprite sprite;
        public float damageMultipler;

        #region Damage
        public float seekInterval;
        public float shootInterval;
        public int shootCount;
        public int bulletCount;
        public BulletSetUp bulletSetUp;
        #endregion

        public TowerSetUp(TowerSetUp other)
        {
            sprite = other.sprite;
            damageMultipler = other.damageMultipler;

            bulletSetUp = new BulletSetUp(other.bulletSetUp);
            bulletCount = other.bulletCount;
            shootCount = other.shootCount;
            seekInterval = other.seekInterval;
            shootInterval = other.shootInterval;
        }
    }
}