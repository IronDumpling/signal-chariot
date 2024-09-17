using UnityEngine;

using InGame.Cores;
using SetUps;

namespace InGame.BattleEffects
{
    public class SplittingEffect : CountEffect
    {
        int m_batchSize;
        BulletSetUp m_bulletSetUp;

        public SplittingEffect(int batchSize, BulletSetUp setUp, int count) : base(count)
        {
            m_batchSize = batchSize;
            m_bulletSetUp = setUp; // splitting effect count -1
        }

        public SplittingEffect(int batchSize, int count) : base(count)
        {
            m_batchSize = batchSize;
        }

        public override void Trigger(GameObject go)
        {
            base.Trigger(go);
            GameManager.Instance.GetBulletManager().AddBulletBatch(m_batchSize, m_bulletSetUp, go.transform);
        }
    }
}