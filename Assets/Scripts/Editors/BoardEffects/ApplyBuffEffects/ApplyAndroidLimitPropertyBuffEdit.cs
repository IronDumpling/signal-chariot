using System;
using System.Collections.Generic;
using InGame.BattleFields.Common;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Effects;
using InGame.Effects.EffectElement.ApplyModuleBuffEffects;
using UnityEngine;

namespace Editors.Effects.ApplyBuffEffects
{
    public class ApplyAndroidLimitPropertyBuffEdit: ApplyModuleBuffEffectEdit
    {
        [System.Serializable]
        private struct buffBlk
        {
            public LimitedPropertyType type;
            public AndroidLimitPropertyBuff.LimitPropertyBuffBlk blk;
        }
        
        [SerializeField]
        private List<buffBlk> buffs;
        public override Effect CreateEffect()
        {

            var newBuff = (AndroidLimitPropertyBuff) AndroidLimitPropertyBuff.CreateBuff();

            foreach (var blk in buffs)
            {
                newBuff.AddProperty(blk.type, blk.blk);
            }

            return ApplyAndroidLimitPropertyBuff.CreateEffect(range, newBuff);
        }
    }
}