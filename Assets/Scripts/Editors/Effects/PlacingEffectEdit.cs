using System.Collections.Generic;
using InGame.Effects;
using InGame.Effects.PlacingEffectRequirements;
using UnityEngine;

namespace Editors.Effects
{
    public class PlacingEffectEdit: MonoBehaviour
    {
        public List<PlacingEffectRequirement.RequirementType> requirements;
        public List<Effect> CreateEffects()
        {
            var effectEdits = transform.GetComponents<EffectEdit>();
            
            var effects = new List<Effect>();

            foreach (var effectEdit in effectEdits)
            {
                effects.Add(effectEdit.CreateEffect());
            }

            return effects;
        }

        public List<PlacingEffectRequirement> CreateRequirements()
        {
            var requirementsList = new List<PlacingEffectRequirement>();

            foreach (var type in requirements)
            {
                requirementsList.Add(PlacingEffectRequirement.CreateRequirement(type));
            }

            return requirementsList;
        }
    }
}