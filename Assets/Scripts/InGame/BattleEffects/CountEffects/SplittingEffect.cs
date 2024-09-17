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
            // foreach(Effect effect in m_bulletSetUp.collisionEffects)
            // {
                // if(effect is SplittingEffect splitting)
                    // splitting.m_count = (splitting.m_count > 0) ? splitting.m_count - 1 : splitting.m_count;
            // }

            // m_bulletSetUp.moveType = MoveType.CircleRound;
            // GameManager.Instance.GetBulletManager().AddBulletBatch(m_batchSize, m_bulletSetUp, go.transform);
        }
    }
}