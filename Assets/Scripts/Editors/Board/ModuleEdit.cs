using System.Collections.Generic;
using Editors.Effects;
using InGame.Boards.Modules;
using InGame.Views;
using SetUps;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace Editors.Board
{
    public class ModuleEdit: MonoBehaviour
    {
        public new string name ="";
        public string desc = "";
        public ModuleView prefab;
        public List<ModulePosition> otherPositions;
        
        private const float CellSize = 1f;

        private string m_prevName = "";
        
        private void OnValidate()
        {
            if (name != m_prevName)
            {
                m_prevName = name;
                gameObject.name = name != "" ? name : "No name";
            }
        }


        private void OnDrawGizmos()
        {
            // always draw the pivot
            Gizmos.color = Color.white;
            Gizmos.DrawCube(transform.position, new Vector3(1,1,1)* CellSize * 0.99f);
            
            Gizmos.color = Color.gray;
            foreach (var pos in otherPositions)
            {
                Gizmos.DrawCube(transform.position + CellSize * new Vector3(pos.x, pos.y), 
                    new Vector3(1,1,1)* CellSize * 0.99f);
            }
        }

        public ModuleSetUp CreateSetUp()
        {
            var signalEffectEdits = GetComponentInChildren<SignalEffectEdit>();
            var placingEffectEdits = GetComponentInChildren<PlacingEffectEdit>();
            
            return new ModuleSetUp
            {
                name = name,
                desc = desc,
                otherPositions = new List<ModulePosition>(otherPositions),
                prefab = prefab,
                signalEffects = signalEffectEdits.CreateEffects(),
                coolDown = signalEffectEdits.coolDown,
                energyConsumption = signalEffectEdits.energyConsumption,
                maxUses = signalEffectEdits.maxUses,
                placingEffects = placingEffectEdits.CreateEffects(),
                requirements = placingEffectEdits.CreateRequirements()
            };
        }
    }
}