using UnityEngine;

using InGame.Cores;
using SetUps;
using InGame.BattleFields.Bullets;

namespace InGame.BattleEffects
{
    public class SplittingEffect : CountEffect
    {
        int m_batchSize;
        BulletSetUp m_bulletSetUp;

        public SplittingEffect(int batchSize, BulletSetUp setUp, int count) : base(count)
        {
            m_batchSize = batchSize;
            m_bulletSetUp = setUp;
        }

        public SplittingEffect(int batchSize, int count) : base(count)
        {
            m_batchSize = batchSize;
        }

        public override void Trigger(GameObject go)
        {
            base.Trigger(go);
            
            BulletSetUp newBullet = new(m_bulletSetUp);
            CountEffect count = null;
            foreach(Effect effect in newBullet.collisionEffects)
            {
                if(effect is CountEffect)
                    count = effect as CountEffect;
                    // splitting.m_count = (splitting.m_count > 0) ? splitting.m_count - 1 : splitting.m_count;
            }

            newBullet.collisionEffects.Remove(count);
            newBullet.moveType = MoveType.CircleRound;
            newBullet.damage /= m_batchSize;
            newBullet.size /= m_batchSize;

            GameManager.Instance.GetBulletManager().AddBulletBatch(m_batchSize, newBullet, go.transform);
        }
    }
}