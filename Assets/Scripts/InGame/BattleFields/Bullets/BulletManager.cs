using System.Collections.Generic;

using UnityEngine;

using SetUps;

using InGame.BattleFields.Androids;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Cores;
using InGame.BattleEffects;

namespace InGame.BattleFields.Bullets
{

    public class BulletManager
    {
        private List<List<Bullet>> m_bullets;
        private List<Vector3> m_targets;

        public BulletManager()
        {
            m_bullets = new();
            m_targets = new();
        }

        public BulletSetUp GenerateBulletSetUp(BulletType bulletType, int bulletLevel, WeaponBuff buff)
        {
            BulletSetUp setUp = GameManager.Instance.GetBulletLib().GetSetUp(bulletType, bulletLevel);
            if(setUp == null)
            {
                Debug.LogError("No " + bulletType.ToString() + " " + bulletLevel + " bullet!");
                return null;
            }
            
            BulletSetUp buffedSetUp = new(setUp);
            buffedSetUp = AddNumericalBuffs(buffedSetUp, buff);
            buffedSetUp = AddEffectiveBuffs(buffedSetUp, buff);
            return buffedSetUp;
        }

        private BulletSetUp AddNumericalBuffs(BulletSetUp bulletSetUp, WeaponBuff buff)
        {
            // life time
            bulletSetUp.lifeTime *= 1 + (float)buff.lifeTimePercentageBuff/100f;
            bulletSetUp.lifeTime = Mathf.Max(0.001f, bulletSetUp.lifeTime);

            // speed
            bulletSetUp.speed *= 1 + (float)buff.speedPercentageBuff/100f;
            bulletSetUp.speed = Mathf.Max(0.001f, bulletSetUp.speed);

            // damage
            // TODO bulletSetUp.damage *= m_damageMultiplier.value;
            bulletSetUp.damage *= 1 + (float)buff.damagePercentageBuff/100f;
            bulletSetUp.damage += buff.flatDamageBuff;
            bulletSetUp.damage = Mathf.Max(0.001f, bulletSetUp.damage);

            // size
            bulletSetUp.size *= 1 + (float)buff.bulletSizePercentageBuff/100f;
            bulletSetUp.size = Mathf.Max(0.001f, bulletSetUp.size);
            return bulletSetUp;
        }

        private BulletSetUp AddEffectiveBuffs(BulletSetUp bulletSetUp, WeaponBuff buff)
        {            
            if(buff.bouncingBuff > 0)
                bulletSetUp.collisionEffects.Add(new BouncingEffect(buff.bouncingBuff));
            if(buff.penetrationBuff > 0)
                bulletSetUp.collisionEffects.Add(new PenetrationEffect(buff.penetrationBuff));
            if(buff.splittingCountBuff > 0)
                bulletSetUp.collisionEffects.Add(new SplittingEffect(buff.splittingSizeBuff, bulletSetUp, buff.splittingCountBuff));
            return bulletSetUp;
        }

        public List<Bullet> AddBulletBatch(float batchSize, BulletSetUp setup, Transform origin)
        {
            List<Bullet> batch = new();
            m_bullets.Add(batch);
            m_targets.Add(Vector3.zero);

            int batchIdx = m_bullets.Count-1;

            for(int i = 0; i < batchSize; i++)
            {
                Bullet bullet = new(setup, origin, new int[2]{batchIdx, i});
                batch.Add(bullet);
            }
            
            return batch;
        }

        public void SetBatchInfo(Vector3 target, int batchIdx)
        {
            m_targets[batchIdx] = target;
        }

        public Vector3 GetBatchInfo(int batchIdx)
        {
            return m_targets[batchIdx];
        }

        public int GetBatchSize(int batchIdx)
        {
            return m_bullets[batchIdx].Count;
        }

        public void ClearBullet()
        {
            foreach(List<Bullet> batch in m_bullets)
                foreach(Bullet bullet in batch)
                    bullet.Die();
            
            m_bullets.Clear();
        }
    }
}