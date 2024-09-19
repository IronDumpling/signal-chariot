using System.Collections.Generic;
using InGame.BattleFields.Common;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Effects;
using InGame.Effects.EffectElement.ApplyModuleBuffEffects;
using UnityEngine;

namespace Editors.Effects.ApplyBuffEffects
{
    public class ApplyAndroidUnlimitedPropertyBuffEdit: ApplyModuleBuffEffectEdit
    {
        [System.Serializable]
        private struct buffBlk
        {
            public UnlimitedPropertyType type;
            public AndroidUnlimitPropertyBuff.UnlimitPropertyBuffBlk blk;
        }
        
        [SerializeField]
        private List<buffBlk> buffs;
        public override Effect CreateEffect()
        {

            var newBuff = (AndroidUnlimitPropertyBuff) AndroidUnlimitPropertyBuff.CreateBuff();

            foreach (var blk in buffs)
            {
                newBuff.AddProperty(blk.type, blk.blk);
            }

            return ApplyAndroidUnlimitedPropertyBuff.CreateEffect(range, newBuff);
        }
    }
    
}