using System.Collections.Generic;
using InGame.BattleFields.Bullets;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Boards.Signals;
using InGame.Cores;
using UnityEngine;
using Utils;

namespace InGame.Effects.EffectElement
{
    public class MagazineShootBulletEffect : Effect
    {
        public int magazineCapacity;
        public override ModuleBuffType buffMask => ModuleBuffType.Weapon | ModuleBuffType.Magazine;
        private WeaponBuff m_weaponBuff = WeaponBuff.CreateBuff();
        private MagazineBuff m_magazineBuff = MagazineBuff.CreateBuff();
        private Queue<SignalType> m_bullets = new();
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            //TODO: maybe the trigger requirement is not just the signal
            if (blackBoard.signal == null) return;
            LoadBullet(blackBoard);
            if (m_bullets.Count == magazineCapacity + m_magazineBuff.magazineCapacityBuff)
            {
                ShootBullets();
            }

            m_module.DisplayProgressBar(m_bullets.Count, magazineCapacity + m_magazineBuff.magazineCapacityBuff);

        }

        private void ShootBullets()
        {
            var equipmentManager = GameManager.Instance.GetAndroid().GetEquipmentManager();
            BulletType curBulletType = default;
            int curLevel = 0;
            while (m_bullets.Count != 0)
            {
                var type = m_bullets.Dequeue();
                var bulletType = Signal.SignalTypeToBulletType(type);

                if (bulletType == curBulletType)
                {
                    curLevel++;
                    if (curLevel == Constants.MAX_BULLET_LEVEL)
                    {
                        equipmentManager.EquipmentEffect(m_module, curBulletType, curLevel, m_weaponBuff.CreateCopy() as WeaponBuff);
                        curLevel = 0;
                    }
                }
                else
                {
                    if (curLevel > 0) equipmentManager.EquipmentEffect(m_module, curBulletType, curLevel, m_weaponBuff.CreateCopy() as WeaponBuff);
                    curBulletType = bulletType;
                    curLevel = 1;
                }
            }
            if (curLevel > 0) equipmentManager.EquipmentEffect(m_module, curBulletType, curLevel, m_weaponBuff.CreateCopy() as WeaponBuff);
        }

        private void LoadBullet(EffectBlackBoard blackBoard)
        {
            var signal = blackBoard.signal;
            
            if(m_bullets.Count < magazineCapacity + m_magazineBuff.magazineCapacityBuff)
            {
                m_bullets.Enqueue(signal.type);
                Debug.Log($"Load Bullet {signal.type}");
            }
        }

        public override void OnAddBuff(ModuleBuff buff)
        {
            if (buff.type == ModuleBuffType.Weapon)
            {
                WeaponBuff weaponBuff = (WeaponBuff) buff;
                m_weaponBuff.Add(weaponBuff);
                Debug.Log(m_weaponBuff);
            }else if (buff.type == ModuleBuffType.Magazine)
            {
                MagazineBuff magazineBuff = (MagazineBuff) buff;
                m_magazineBuff.Add(magazineBuff);
            }            
        }
        
        public override void OnRemoveBuff(ModuleBuff buff)
        {
            if (buff.type == ModuleBuffType.Weapon)
            {
                WeaponBuff weaponBuff = (WeaponBuff) buff;
                m_weaponBuff.Minus(weaponBuff);
                Debug.Log(m_weaponBuff);
            }else if (buff.type == ModuleBuffType.Magazine)
            {
                MagazineBuff magazineBuff = (MagazineBuff) buff;
                m_magazineBuff.Minus(magazineBuff);
            }
            
        }

        public override void ClearBuffs()
        {
            m_weaponBuff.SetDefault();
            m_magazineBuff.SetDefault();
            Debug.Log(m_weaponBuff);
        }
        
        public override Effect CreateCopy()
        {
            return new MagazineShootBulletEffect
            {
                magazineCapacity = magazineCapacity
            };
        }

        public static Effect CreateEffect(int capacity)
        {
            return new MagazineShootBulletEffect
            {
                magazineCapacity = capacity
            };
        }
        
    }
}