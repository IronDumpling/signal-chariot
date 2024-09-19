using UnityEngine;
using Utils;

namespace InGame.BattleFields.Common
{
    public class ModManager
    {
        private readonly Transform m_modsTransform;
        private int m_costIncrement;

        public ModManager()
        {
            m_modsTransform = new GameObject("Mods").transform;
        }
        
        public Mod CreateMod(int quality, Vector2 position)
        {
            var newMod = new Mod(quality, position);
            newMod.view.transform.parent = m_modsTransform;
            return newMod;
        }

        public int GetAddSlotCost()
        {
            return Constants.ADD_SLOT_COST_BASE + m_costIncrement;
        }

        public void IncrementAddSlotCost()
        {
            m_costIncrement += Constants.ADD_SLOT_COST_INCRE;
        }
    }
}