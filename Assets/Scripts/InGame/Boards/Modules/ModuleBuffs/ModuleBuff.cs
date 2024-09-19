using System;
using System.Collections.Generic;

namespace InGame.Boards.Modules.ModuleBuffs
{
    [Flags]
    public enum ModuleBuffType
    {
        None = 0,
        Weapon = 1 << 0,
        Magazine = 1 << 1,
        Heat = 1 << 2,
        AndroidLimitProperty = 1 << 3,
        AndroidUnlimitProperty = 1 << 4
        
    }


    
    public abstract class ModuleBuff
    {

        public abstract ModuleBuffType type { get; }
        public abstract ModuleBuff CreateCopy();

        public void Add(ModuleBuff other)
        {
            if (other.type != type) return;
            OnAdd(other);
        }

        protected abstract void OnAdd(ModuleBuff other);

        public void Minus(ModuleBuff other)
        {
            if (other.type != type) return;
            OnMinus(other);
        }
        
        protected abstract void OnMinus(ModuleBuff other);

        public abstract void SetDefault();
        
        public static List<ModuleBuffType> GetBuffTypeList(ModuleBuffType mask)
        {
            List<ModuleBuffType> selectedOptions = new List<ModuleBuffType>();

            foreach (ModuleBuffType value in Enum.GetValues(typeof(ModuleBuffType)))
            {
                if (value != ModuleBuffType.None && mask.HasFlag(value))
                {
                    selectedOptions.Add(value);
                }
            }

            return selectedOptions;
        }

        public static bool IsInMask(ModuleBuffType mask, ModuleBuffType buff)
        {
            var list = GetBuffTypeList(mask);
            foreach (var type in list)
            {
                if (buff == type) return true;
            }

            return false;
        }

        public static ModuleBuff CreateEmptyBuff(ModuleBuffType type)
        {
            return type switch
            {
                ModuleBuffType.None => null,
                ModuleBuffType.Weapon => WeaponBuff.CreateBuff(),
                ModuleBuffType.Magazine => MagazineBuff.CreateBuff(),
                ModuleBuffType.AndroidLimitProperty => AndroidLimitPropertyBuff.CreateEmptyBuff(),
                ModuleBuffType.AndroidUnlimitProperty => AndroidUnlimitPropertyBuff.CreateEmptyBuff(),
                _ => null
            };
        }
    }
}