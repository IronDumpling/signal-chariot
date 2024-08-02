using System.Collections.Generic;
using InGame.Boards.Modules;
using InGame.Effects;
using InGame.Effects.PlacingEffectRequirements;
using InGame.Views;
using UnityEngine;

namespace SetUps
{
    [System.Serializable]
    public class ModuleSetUp
    {
        public string name ="";
        public string desc = "";
        public List<ModulePosition> otherPositions;
        public ModuleView prefab;
        
        #region SignalEffects

        [SerializeReference]
        public List<Effect> signalEffects;
        public int maxUses;
        public int energyConsumption;
        public float coolDown;
        #endregion
        
        #region PlacingEffects
        
        [SerializeReference]
        public List<Effect> placingEffects;
        
        [SerializeReference]
        public List<PlacingEffectRequirement> requirements;

        #endregion
    }
}