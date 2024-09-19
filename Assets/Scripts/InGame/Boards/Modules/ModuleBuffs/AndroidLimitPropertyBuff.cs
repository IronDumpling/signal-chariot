using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InGame.BattleFields.Common;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.Boards.Modules.ModuleBuffs
{
    [System.Serializable]
    public class AndroidLimitPropertyBuff: ModuleBuff
    {
        [System.Serializable]
        public struct LimitPropertyBuffBlk
        {
            public float currentValueBuff;
            public float maxValueBuff;
            
            private const float Tolerance = 1e-7f;
            
            public LimitPropertyBuffBlk(float currentValueBuff, float maxValueBuff)
            {
                this.currentValueBuff = currentValueBuff;
                this.maxValueBuff = maxValueBuff;
            }
            
            public static LimitPropertyBuffBlk operator+ (LimitPropertyBuffBlk a, LimitPropertyBuffBlk b)
            {
                var newBlk =  new LimitPropertyBuffBlk(a.currentValueBuff + b.currentValueBuff,
                    a.maxValueBuff + b.maxValueBuff);
                if (Mathf.Abs(newBlk.currentValueBuff) <= Tolerance) newBlk.currentValueBuff = 0f;
                if (Mathf.Abs(newBlk.maxValueBuff) <= Tolerance) newBlk.maxValueBuff = 0f;

                return newBlk;
            }
            
            public static LimitPropertyBuffBlk operator- (LimitPropertyBuffBlk a, LimitPropertyBuffBlk b)
            {
                var newBlk =  new LimitPropertyBuffBlk(a.currentValueBuff - b.currentValueBuff,
                    a.maxValueBuff - b.maxValueBuff);

                if (Mathf.Abs(newBlk.currentValueBuff) <= Tolerance) newBlk.currentValueBuff = 0f;
                if (Mathf.Abs(newBlk.maxValueBuff) <= Tolerance) newBlk.maxValueBuff = 0f;

                return newBlk;
            }

            public static LimitPropertyBuffBlk zero => new LimitPropertyBuffBlk(0, 0);
        }
        
        public override ModuleBuffType type => ModuleBuffType.AndroidLimitProperty;

        public List<LimitedPropertyType> types = new List<LimitedPropertyType>();
        public List<LimitPropertyBuffBlk> blks = new List<LimitPropertyBuffBlk>();
        public Dictionary<LimitedPropertyType, LimitPropertyBuffBlk> buffs = null;
        
        public override ModuleBuff CreateCopy()
        {
            if (buffs == null)
            {
                buffs = new Dictionary<LimitedPropertyType, LimitPropertyBuffBlk>();
                for (int i = 0; i < types.Count; i++)
                {
                    buffs.Add(types[i], blks[i]);
                }
            }
            
            var newBuff = new AndroidLimitPropertyBuff();
            newBuff.buffs = new Dictionary<LimitedPropertyType, LimitPropertyBuffBlk>();
            foreach (var keyValue in buffs)
            {
                var type = keyValue.Key;
                var blk = keyValue.Value;
                newBuff.buffs.Add(type, blk);
            }

            return newBuff;
        }

        protected override void OnAdd(ModuleBuff other)
        {
            var otherBuff = (AndroidLimitPropertyBuff)other;
            
            foreach (var (propertyType, blk) in otherBuff.buffs)
            {
                if (!buffs.ContainsKey(propertyType)) buffs.Add(propertyType, LimitPropertyBuffBlk.zero);

                buffs[propertyType] += blk;
            }
        }

        protected override void OnMinus(ModuleBuff other)
        {
            var otherBuff = (AndroidLimitPropertyBuff)other;
            
            foreach (var (propertyType, blk) in otherBuff.buffs)
            {
                if (!buffs.ContainsKey(propertyType)) buffs.Add(propertyType, LimitPropertyBuffBlk.zero);

                buffs[propertyType] -= blk;
            }
        }

        public override void SetDefault()
        {
            var keys = new List<LimitedPropertyType>(buffs.Keys);
            foreach (var propertyType in keys)
            {
                buffs[propertyType] = LimitPropertyBuffBlk.zero;
            }
        }

        public bool GetBlk(LimitedPropertyType propertyType, out LimitPropertyBuffBlk blk)
        {
            if (!buffs.ContainsKey(propertyType))
            {
                blk = LimitPropertyBuffBlk.zero;
                return false;
            }
            else
            {
                blk = buffs[propertyType];
                return true;
            }
            
        }
        public void AddProperty(LimitedPropertyType propertyType, LimitPropertyBuffBlk blk)
        {
            types.Add(propertyType);
            blks.Add(blk);
            //buffs[propertyType] = blk;
        }

        public static ModuleBuff CreateEmptyBuff()
        {
            return new AndroidLimitPropertyBuff
            {
                buffs = new Dictionary<LimitedPropertyType, LimitPropertyBuffBlk>()
            };
        }
        
        /// <summary>
        /// This can only used for editor, if you want to create a buff in game use CreateEmptyBuff
        /// </summary>
        /// <returns></returns>
        public static ModuleBuff CreateBuff()
        {
            return new AndroidLimitPropertyBuff();
        }
    }
    
}