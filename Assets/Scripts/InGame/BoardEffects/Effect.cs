using System.Collections.Generic;
using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Boards.Signals;
using InGame.Effects.PlacingEffectRequirements;
using InGame.Effects.TriggerRequirements;
using UnityEngine;
using Time = InGame.Boards.Signals.Time;

namespace InGame.Effects
{
    public class EffectBlackBoard
    {
        public TriggerType triggerType;
        
        // signal Effect
        public Signal signal;
        public BoardPosition pos;
        public Time time;
        public int energyUsed=0;
        
        //placing effect
        public Slot slot;
        public Module module;
        
        
        // test
        public bool isTest = false;
        public void Clean()
        {
            signal = null;

            slot = null;
            module = null;
        }
    }
    [System.Serializable]
    public abstract class Effect
    {
        protected Module m_module;

        public virtual ModuleBuffType buffMask => ModuleBuffType.None;        
        // To DO: Maybe move this to the Signal Effect because Only signal effect distinguish this
        protected virtual bool canEffectByTest => false;
            
        public void SetModule(Module module) => m_module = module;

        public Module GetModule(Module module) => m_module;

        public void Trigger(EffectBlackBoard blackBoard)
        {
            if (blackBoard.isTest && !canEffectByTest) return;
            OnTrigger(blackBoard);
        }
        
        public abstract void OnTrigger(EffectBlackBoard blackBoard);

        public void UnTrigger(EffectBlackBoard blackBoard)
        {
            OnUnTrigger(blackBoard);
        }
        
        public virtual void OnUnTrigger(EffectBlackBoard blackBoard){}

        public void Reset()
        {
            OnReset();
        }

        protected virtual void OnReset()
        {
            
        }

        public void AddBuff(ModuleBuff buff)
        {
            if (ModuleBuff.IsInMask(buffMask, buff.type))
            {
                OnAddBuff(buff);
            }
        }
        
        public virtual void OnAddBuff(ModuleBuff buff){}

        public void RemoveBuff(ModuleBuff buff)
        {
            if (ModuleBuff.IsInMask(buffMask, buff.type))
            {
                OnRemoveBuff(buff);
            }
        }
        
        public virtual void OnRemoveBuff(ModuleBuff buff){}
        public virtual void ClearBuffs(){}
        public abstract Effect CreateCopy();
    }

    public class SignalEffects
    {
        public enum EnergyConsumptionMethod
        {
            Fixed,
            Stored,
            All
        }

        private Module m_module;
        private List<Effect> m_effects;
        private int m_maxUses, m_remainUses;

        private EnergyConsumptionMethod m_consumptionMethod = EnergyConsumptionMethod.Fixed;
        private int m_energyConsumption;
        private int m_storedEnergy = 0;

        private int storedEnergy
        {
            get => m_storedEnergy;
            set
            {
                m_storedEnergy = value;
                m_module.DisplayProgressBar(m_storedEnergy, m_energyConsumption);
            }
        }
        private int m_maxStoredPerTrigger = 0;
        
        private Time m_coolDown;
        private Time m_prevTriggerTime;

        private SignalType m_signalMask;

        #region Constructor
        public static SignalEffects CreateSignalEffects(List<Effect> signalEffects, int maxUses,
        EnergyConsumptionMethod consumptionMethod, int energyConsumption, int maxStoredPerTrigger, float coolDown, SignalType mask)
        {
            return new SignalEffects
            {
                m_effects = new List<Effect>(signalEffects),
                m_maxUses = maxUses,
                m_remainUses = maxUses,
                m_consumptionMethod = consumptionMethod,
                m_energyConsumption = energyConsumption,
                m_maxStoredPerTrigger = maxStoredPerTrigger,
                m_coolDown = new Time(coolDown),
                m_prevTriggerTime = new Time(-coolDown),
                m_signalMask = mask
            };
        }
        
        public static SignalEffects CreateSignalEffects(List<Effect> signalEffects, int maxUses, EnergyConsumptionMethod consumptionMethod,
            int energyConsumption, int maxStoredPerTrigger, Time coolDown, SignalType mask)
        {
            return new SignalEffects
            {
                m_effects = new List<Effect>(signalEffects),
                m_maxUses = maxUses,
                m_remainUses = maxUses,
                m_consumptionMethod = consumptionMethod,
                m_energyConsumption = energyConsumption,
                m_maxStoredPerTrigger = maxStoredPerTrigger,
                m_coolDown = coolDown,
                m_prevTriggerTime = -coolDown,
                m_signalMask = mask
            };
        }

        public static SignalEffects CreateSignalEffects(SignalEffects other)
        {
            var signalEffects = new List<Effect>();
            foreach (var eff in other.m_effects)
            {
                signalEffects.Add(eff.CreateCopy());
            }


            return CreateSignalEffects(signalEffects, other.m_maxUses, other.m_consumptionMethod,
                other.m_energyConsumption, other.m_maxStoredPerTrigger, other.m_coolDown, other.m_signalMask);
        }
        #endregion
        
        public void Trigger(EffectBlackBoard blackBoard)
        {
            var signal = blackBoard.signal;
            var time = blackBoard.time;
            
            // filter out certain type of signal
            if ((m_signalMask & signal.type) != signal.type) return;
            
            // // To Do: maybe every type of signal will have its own consume mechanism
            // if (signal.type != SignalType.Normal)
            // {
            //     foreach (var effect in m_effects)
            //     {
            //         effect.Trigger(blackBoard);
            //     }
            //
            //     return;
            // }
            
            // cool down not finished
            if (time - m_prevTriggerTime < m_coolDown)
            {
                Debug.Log($"coolDown not passed {time - m_prevTriggerTime} and {m_coolDown}");
                return;
            }

            if (m_maxUses != -1 && m_remainUses <= 0)
            {
                Debug.Log("Uses not passed");
                return;
            }

            int energyUsed;
            if (m_consumptionMethod == EnergyConsumptionMethod.Fixed)
            {
                if (signal.energy < m_energyConsumption)
                {
                    Debug.Log("Energy not passed");
                    return;
                }
                signal.ConsumeEnergy(m_energyConsumption);
                energyUsed = m_energyConsumption;
            }else if (m_consumptionMethod == EnergyConsumptionMethod.Stored)
            {
                int consumeEnergy = Mathf.Min(m_maxStoredPerTrigger, signal.energy);
                signal.ConsumeEnergy(consumeEnergy);
                storedEnergy += consumeEnergy;
                
                if (storedEnergy < m_energyConsumption) return;
                
                energyUsed = m_energyConsumption;
                storedEnergy -= energyUsed;
            }
            else if (m_consumptionMethod == EnergyConsumptionMethod.All)
            {
                energyUsed = signal.energy;
                signal.ConsumeEnergy(energyUsed);
            }
            else
            {
                return;
            }
            
            
            if(m_maxUses != -1) m_remainUses--;

            m_prevTriggerTime = time;
            blackBoard.energyUsed = energyUsed;
            
            m_module.PlayAnimation("TriggerAnimation");
            foreach (var effect in m_effects)
            {
                effect.Trigger(blackBoard);
            }
            
        }

        public void Reset()
        {
            storedEnergy = 0;
            foreach (var effect in m_effects)
            {
                effect.Reset();
            }
        }
        
        public void AddBuff(ModuleBuff buff)
        {
            foreach(var effect in m_effects) effect.AddBuff(buff);
        }
        
        public void RemoveBuff(ModuleBuff buff)
        {
            foreach(var effect in m_effects) effect.RemoveBuff(buff);
        }
        
        public void ClearBuffs(){
            foreach(var effect in m_effects) effect.ClearBuffs();
            
        }
        
        public void SetModule(Module module)
        {
            m_module = module;
            foreach(var effect in m_effects) effect.SetModule(module);
        }
    }


    public class PlacingEffects
    {
        private List<Effect> m_effects;
        private List<PlacingEffectRequirement> m_requirements;
        private bool m_isTrigger = false;

        public static PlacingEffects CreatePlacingEffects(List<Effect> effect, List<PlacingEffectRequirement> reqs)
        {
            return new PlacingEffects
            {
                m_effects = new List<Effect>(effect),
                m_requirements = reqs
            };
        }
        
        public static PlacingEffects CreatePlacingEffects(PlacingEffects other)
        {
            var placingEffects = new List<Effect>();
            foreach (var eff in other.m_effects)
            {
                placingEffects.Add(eff.CreateCopy());
            }

            var requirements = new List<PlacingEffectRequirement>();
            foreach (var requirement in other.m_requirements)
            {
                requirements.Add(requirement.CreateCopy());
            }
            return new PlacingEffects
            {
                m_effects = placingEffects,
                m_requirements = requirements,
                
            };
        }
        
        public void Trigger(EffectBlackBoard blackBoard)
        {
            foreach (var req in m_requirements)
            {
                if (!req.CanTrigger(blackBoard))
                {
                    return;
                }
            }
            
            m_isTrigger = true;
            
            foreach (var effect in m_effects)
            {
                effect.Trigger(blackBoard);
            }
        }

        public void UnTrigger(EffectBlackBoard blackBoard)
        {
            if (!m_isTrigger) return;
            
            foreach (var effect in m_effects)
            {
                effect.UnTrigger(blackBoard);
            }

            m_isTrigger = false;
        }
        
        public void Reset()
        {
            foreach (var effect in m_effects)
            {
                effect.Reset();
            }
        }
        
        public void AddBuff(ModuleBuff buff)
        {
            foreach(var effect in m_effects) effect.AddBuff(buff);
        }
        
        public void RemoveBuff(ModuleBuff buff)
        {
            foreach(var effect in m_effects) effect.RemoveBuff(buff);
        }
        
        public void ClearBuffs(){
            foreach(var effect in m_effects) effect.ClearBuffs();
            
        }
        
        public void SetModule(Module module)
        {
            foreach(var effect in m_effects) effect.SetModule(module);
        }
    }

    public class CustomEffect
    {
        private TriggerRequirement m_requirement;
        private List<Effect> m_effects;
        private Module m_module;

        public void Register(RequirementBlackBoard bb)
        {
            m_requirement.RegisterTriggerEvent(Trigger);
            m_requirement.Register(bb);
        }

        public void Unregister(RequirementBlackBoard bb)
        {
            m_requirement.Unregister(bb);
            m_requirement.UnregisterTriggerEvent(Trigger);
        }
        
        public void SetModule(Module module)
        {
            m_module = module;
            m_requirement.SetModule(module);
            foreach(var effect in m_effects) effect.SetModule(module);
        }
        
        private void Trigger(EffectBlackBoard blackBoard)
        {
            foreach (var effect in m_effects)
            {
                effect.Trigger(blackBoard);
            }
        }
        
        public void Reset()
        {
            foreach (var effect in m_effects)
            {
                effect.Reset();
            }
        }
        
        public void AddBuff(ModuleBuff buff)
        {
            m_requirement.AddBuff(buff);
            foreach(var effect in m_effects) effect.AddBuff(buff);
        }
        
        public void RemoveBuff(ModuleBuff buff)
        {
            m_requirement.RemoveBuff(buff);
            foreach(var effect in m_effects) effect.RemoveBuff(buff);
        }
        
        public void ClearBuffs()
        {
            m_requirement.ClearBuffs();
            foreach(var effect in m_effects) effect.ClearBuffs();
            
        }
        
        public static CustomEffect CreateCustomEffect(TriggerRequirement requirement, List<Effect> effects)
        {
            return new CustomEffect
            {
                m_requirement = requirement,
                m_effects = effects
            };
        }
        
        public static CustomEffect CreateCustomEffect(CustomEffect other)
        {
            if (other == null) return null;

            var customEffectLists = new List<Effect>();
            var triggerRequirement = other.m_requirement.CreateCopy();

            foreach (var effect in other.m_effects)
            {
                customEffectLists.Add(effect.CreateCopy());
            }
            
            return new CustomEffect
            {
                m_requirement = triggerRequirement,
                m_effects = customEffectLists
            };
        }
    }
}