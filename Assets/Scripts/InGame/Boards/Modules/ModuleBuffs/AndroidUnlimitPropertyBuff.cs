using System.Collections.Generic;
using InGame.BattleFields.Common;
using UnityEngine;
using UnityEngine.Serialization;

// ToDo: Maybe combine limit and unlimit into generic class
namespace InGame.Boards.Modules.ModuleBuffs
{
    [System.Serializable]
    public class AndroidUnlimitPropertyBuff: ModuleBuff
    {
        [System.Serializable]
        public struct UnlimitPropertyBuffBlk
        {
            public float valueBuff;
            
            private const float Tolerance = 1e-7f;
            
            public UnlimitPropertyBuffBlk(float valueBuff)
            {
                this.valueBuff = valueBuff;
            }
            
            public static UnlimitPropertyBuffBlk operator+ (UnlimitPropertyBuffBlk a, UnlimitPropertyBuffBlk b)
            {
                var newBlk =  new UnlimitPropertyBuffBlk(a.valueBuff + b.valueBuff);
                if (Mathf.Abs(newBlk.valueBuff) <= Tolerance) newBlk.valueBuff = 0f;

                return newBlk;
            }
            
            public static UnlimitPropertyBuffBlk operator- (UnlimitPropertyBuffBlk a, UnlimitPropertyBuffBlk b)
            {
                var newBlk =  new UnlimitPropertyBuffBlk(a.valueBuff - b.valueBuff);

                if (Mathf.Abs(newBlk.valueBuff) <= Tolerance) newBlk.valueBuff = 0f;

                return newBlk;
            }

            public static UnlimitPropertyBuffBlk zero => new UnlimitPropertyBuffBlk(0);
        }
        
        public override ModuleBuffType type => ModuleBuffType.AndroidUnlimitProperty;
        public List<UnlimitedPropertyType> types = new List<UnlimitedPropertyType>();
        public List<UnlimitPropertyBuffBlk> blks = new List<UnlimitPropertyBuffBlk>();
        private Dictionary<UnlimitedPropertyType, UnlimitPropertyBuffBlk> buffs = null;
        
        public override ModuleBuff CreateCopy()
        {
            if (buffs == null)
            {
                buffs = new Dictionary<UnlimitedPropertyType, UnlimitPropertyBuffBlk>();
                for (int i = 0; i < types.Count; i++)
                {
                    buffs.Add(types[i], blks[i]);
                }
            }
            
            var newBuff = new AndroidUnlimitPropertyBuff();
            newBuff.buffs = new Dictionary<UnlimitedPropertyType, UnlimitPropertyBuffBlk>();
            foreach (var keyValue in buffs)
            {
                var unlimitedPropertyType = keyValue.Key;
                var blk = keyValue.Value;
                newBuff.buffs.Add(unlimitedPropertyType, blk);
            }

            return newBuff;
        }

        protected override void OnAdd(ModuleBuff other)
        {
            var otherBuff = (AndroidUnlimitPropertyBuff)other;
            
            foreach (var (propertyType, blk) in otherBuff.buffs)
            {
                if (!buffs.ContainsKey(propertyType)) buffs.Add(propertyType, UnlimitPropertyBuffBlk.zero);

                buffs[propertyType] += blk;
            }
        }

        protected override void OnMinus(ModuleBuff other)
        {
            var otherBuff = (AndroidUnlimitPropertyBuff)other;
            
            foreach (var (propertyType, blk) in otherBuff.buffs)
            {
                if (!buffs.ContainsKey(propertyType)) buffs.Add(propertyType, UnlimitPropertyBuffBlk.zero);

                buffs[propertyType] -= blk;
            }
        }

        public override void SetDefault()
        {
            var keys = new List<UnlimitedPropertyType>(buffs.Keys);
            foreach (var propertyType in keys)
            {
                buffs[propertyType] = UnlimitPropertyBuffBlk.zero;
            }
        }
        
        public bool GetBlk(UnlimitedPropertyType propertyType, out UnlimitPropertyBuffBlk blk)
        {
            if (!buffs.ContainsKey(propertyType))
            {
                blk = UnlimitPropertyBuffBlk.zero;
                return false;
            }
            else
            {
                blk = buffs[propertyType];
                return true;
            }
            
        }
        
        public void AddProperty(UnlimitedPropertyType propertyType, UnlimitPropertyBuffBlk blk)
        {
            types.Add(propertyType);
            blks.Add(blk);
            //buffs[propertyType] = blk;
        }
        
        public static ModuleBuff CreateEmptyBuff()
        {
            return new AndroidUnlimitPropertyBuff
            {
                buffs = new Dictionary<UnlimitedPropertyType, UnlimitPropertyBuffBlk>()
            };
        }
        
        /// <summary>
        /// This can only used for editor, if you want to create a buff in game use CreateEmptyBuff
        /// </summary>
        /// <returns></returns>
        public static ModuleBuff CreateBuff()
        {
            return new AndroidUnlimitPropertyBuff();
        }
    }
}