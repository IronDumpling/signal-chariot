using System.Collections.Generic;

using UnityEngine;

using SetUps;

namespace InGame.BattleFields.Chariots
{
    public class BulletManager
    {
        private List<Bullet> m_bullets;

        public BulletManager()
        {
            m_bullets = new();
        }

        public Bullet AddBullet(BulletSetUp setup, Vector3 target, float dmgMultiplier)
        {
            Bullet bullet = new(setup, target, dmgMultiplier);
            m_bullets.Add(bullet);
            return bullet;
        }

        public void RemoveBullet(Bullet bullet)
        {
            m_bullets.Remove(bullet);
            bullet.Die();
        }
    }
}