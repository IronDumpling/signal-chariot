using InGame.Boards.Modules.ModuleBuffs;
using InGame.Boards.Signals;
using InGame.Cores;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.Effects.EffectElement
{
    public class WeaponShootEffect: Effect
    {
        public override ModuleBuffType buffMask => ModuleBuffType.Weapon;
        public float heatGauge;
        public float dissipationRate;
        public float heatCostPerShot;
        public int electricCapacity;
        public int electricChargeGainPerTrigger;

        private int m_currentElectricCharge = 0;
        private int currentElectricCharge
        {
            get => m_currentElectricCharge;
            set
            {
                m_currentElectricCharge = value;
                m_module.DisplayProgressBar(m_currentElectricCharge, electricCapacity);
            }
        }
        private float m_currentHeat = 0;
        private bool m_isOverHeated = false;
        private float m_lastRecordedTime = -1f;
        private WeaponBuff m_buff = WeaponBuff.CreateBuff();

        private void AddHeat(float delta)
        {
            m_currentHeat += delta;
            if (m_currentHeat >= heatGauge)
            {
                m_currentHeat = heatGauge;
                m_isOverHeated = true;
            }
        }

        private void ReduceHeat(float delta)
        {
            m_currentHeat -= delta;
            if (m_currentHeat <= 0)
            {
                m_currentHeat = 0;
                m_isOverHeated = false;
            }
        }

        private void UpdateHeat(float currentTime)
        {
            float delta = currentTime - m_lastRecordedTime;
            m_lastRecordedTime = currentTime;
            if (m_currentHeat <= Mathf.Epsilon) return;
            Debug.Assert(delta >= 0);

            float dissipationEnergy = dissipationRate * delta;
            ReduceHeat(dissipationEnergy);
        }

        private void IncreaseElectricCharge(int delta)
        {
            currentElectricCharge = Mathf.Clamp(currentElectricCharge + delta, 0, electricCapacity);
        }

        private void ClearElectricCharge()
        {
            currentElectricCharge = 0;
        }

        private int GetCurrentElectricCharge() => currentElectricCharge;
        

        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            
            UpdateHeat(blackBoard.time.val);
            
            if (m_isOverHeated) return;

            var signalType = blackBoard.signal.type;
            var bulletType = Signal.SignalTypeToBulletType(signalType);
            
            AddHeat(heatCostPerShot);

            int bulletLevel = 1;
            if (GetCurrentElectricCharge() == electricCapacity)
            {
                bulletLevel++;
                ClearElectricCharge();
            }
            else
            {
                IncreaseElectricCharge(electricChargeGainPerTrigger);
            }
            
            GameManager.Instance.GetAndroid().GetEquipmentManager().
            EquipmentEffect(m_module, bulletType, bulletLevel, m_buff.CreateCopy() as WeaponBuff);
        }

        protected override void OnReset()
        {
            m_currentHeat = 0f;
            m_isOverHeated = false;
            m_lastRecordedTime = -1f;
            currentElectricCharge = 0;
        }

        public override void OnAddBuff(ModuleBuff buff)
        {
            WeaponBuff weaponBuff = (WeaponBuff) buff;
            m_buff.Add(weaponBuff);
            Debug.Log(m_buff);
        }
        
        public override void OnRemoveBuff(ModuleBuff buff)
        {
            WeaponBuff weaponBuff = (WeaponBuff) buff;
            m_buff.Minus(weaponBuff);
            Debug.Log(m_buff);
        }

        public override void ClearBuffs()
        {
            m_buff.SetDefault();
        }

        public override Effect CreateCopy()
        {
            return new WeaponShootEffect
            {
                heatGauge = heatGauge,
                dissipationRate = dissipationRate,
                heatCostPerShot = heatCostPerShot,
                electricCapacity = electricCapacity,
                electricChargeGainPerTrigger = electricChargeGainPerTrigger
            };
        }
        

        public static WeaponShootEffect CreateEffect(float heatGauge, float dissipationRate, float heatCostPerShot, 
            int electricCapacity, int electricChargeGainPerTrigger)
        {
            return new WeaponShootEffect
            {
                heatGauge = heatGauge,
                dissipationRate = dissipationRate,
                heatCostPerShot = heatCostPerShot,
                electricCapacity = electricCapacity,
                electricChargeGainPerTrigger = electricChargeGainPerTrigger
            };
        }
    }
}