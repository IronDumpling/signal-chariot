using InGame.BattleFields.Chariots;
using InGame.Cores;
using SetUps;
using UnityEngine;

namespace InGame.Effects
{
    public class PlacingTowerEffect: Effect
    {
        public TowerSetUp setUp;
        private Tower m_tower;
        
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            m_tower = GameManager.Instance.GetChariot().GetTowerManager().AddTower(setUp, m_module);
        }

        public override void OnUnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetChariot().GetTowerManager().RemoveTower(m_tower);
        }

        public override Effect CreateCopy()
        {
            return new PlacingTowerEffect
            {
                setUp = setUp
            };
        }

        public static PlacingTowerEffect CreateEffect(TowerSetUp newSetUp)
        {
            return new PlacingTowerEffect
            {
                setUp = newSetUp
            };
        }
    }
}