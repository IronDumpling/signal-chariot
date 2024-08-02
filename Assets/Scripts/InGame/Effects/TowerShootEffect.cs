using InGame.Cores;

namespace InGame.Effects
{
    public class TowerShootEffect: Effect
    {
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetChariot().GetTowerManager().TowerEffect(m_module);
        }

        public override Effect CreateCopy()
        {
            return new TowerShootEffect();
        }

        public static TowerShootEffect CreateEffect()
        {
            return new TowerShootEffect();
        }
    }
}