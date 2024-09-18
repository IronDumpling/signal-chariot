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
            foreach(Effect effect in newBullet.collisionEffects)
            {
                if(effect is SplittingEffect splitting)
                {
                    splitting.m_count = (splitting.m_count > 0) ? splitting.m_count - 1 : splitting.m_count;
                    Debug.Log("new splitting effect " + splitting.m_count);
                }
            }

            newBullet.moveType = MoveType.CircleRound;
            newBullet.damage /= m_batchSize;
            newBullet.size /= m_batchSize;
            Debug.Log("new bullet setup, damage: " + newBullet.damage + 
                    "; size: " + newBullet.size + " collision effect count: " + 
                    newBullet.collisionEffects.Count);
            GameManager.Instance.GetBulletManager().AddBulletBatch(m_batchSize, m_bulletSetUp, go.transform);
        }
    }
}