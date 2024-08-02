using System;

using UnityEngine;

using InGame.BattleFields.Chariots;

namespace SetUps
{
    [Serializable]
    public class TowerSetUp
    {
        public Sprite sprite;
        public float damageMultipler;

        #region Damage
        public float seekInterval;
        public SeekMode seekMode;
        public float shootInterval;
        public int bulletCount;
        public BulletSetUp bulletSetUp;
        #endregion

        public TowerSetUp(TowerSetUp other)
        {
            sprite = other.sprite;
            damageMultipler = other.damageMultipler;

            bulletSetUp = new BulletSetUp(other.bulletSetUp);
            bulletCount = other.bulletCount;
            seekInterval = other.seekInterval;
            shootInterval = other.shootInterval;
            seekMode = other.seekMode;
        }
    }
}