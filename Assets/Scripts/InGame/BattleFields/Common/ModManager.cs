using UnityEngine;
using UnityEngine.Events;

using Utils;

namespace InGame.BattleFields.Common
{
    public class ModManager
    {
        private readonly Transform m_modsTransform;
        private UnlimitedProperty m_modCost;

        public ModManager()
        {
            m_modsTransform = new GameObject("Mods").transform;
            m_modCost = new UnlimitedProperty(Constants.ADD_SLOT_COST_BASE);
        }
        
        public Mod CreateMod(int quality, Vector2 position)
        {
            var newMod = new Mod(quality, position);
            newMod.view.transform.parent = m_modsTransform;
            return newMod;
        }

        public int GetAddSlotCost()
        {
            return (int)m_modCost.value;
        }

        public void IncrementAddSlotCost()
        {
            m_modCost.value += Constants.ADD_SLOT_COST_INCRE;
        }

        public void RegisterModCostEvent(UnityAction<float> call)
        {
            m_modCost.onValueChanged.AddListener(call);
            m_modCost.onValueChanged.Invoke(GetAddSlotCost());
        }

        public void UnregisterModCostEvent(UnityAction<float> call)
        {
            m_modCost.onValueChanged.RemoveListener(call);
        }
    }
}