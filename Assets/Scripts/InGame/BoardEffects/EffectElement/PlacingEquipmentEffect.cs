using InGame.BattleFields.Androids;
using InGame.Cores;
using SetUps;
using UnityEngine;

namespace InGame.Effects.EffectElement
{
    public class PlacingEquipmentEffect: Effect
    {
        public EquipmentSetUp setUp;
        private Equipment m_equipment;
        
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            m_equipment = GameManager.Instance.GetAndroid().GetEquipmentManager().AddEquipment(setUp, m_module);
        }

        public override void OnUnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().GetEquipmentManager().RemoveEquipment(m_equipment);
        }

        public override Effect CreateCopy()
        {
            return new PlacingEquipmentEffect
            {
                setUp = setUp
            };
        }

        public static PlacingEquipmentEffect CreateEffect(EquipmentSetUp newSetUp)
        {
            return new PlacingEquipmentEffect
            {
                setUp = newSetUp
            };
        }
    }
}