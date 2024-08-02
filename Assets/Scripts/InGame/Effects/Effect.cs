using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Boards.Signals;
using InGame.Effects.PlacingEffectRequirements;
using UnityEngine;
using Time = InGame.Boards.Signals.Time;

namespace InGame.Effects
{
    public class EffectBlackBoard
    {
        // signal Effect
        public Signal signal;
        public Time time;
        
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
        
        public virtual void OnUnTrigger(EffectBlackBoard blackBoard)
        {
        }

        public abstract Effect CreateCopy();
    }

    public class SignalEffects
    {
        private List<Effect> m_effects;
        private int m_maxUses, m_remainUses;
        private int m_energyConsumption;
        
        private Time m_coolDown;
        private Time m_prevTriggerTime;

        public static SignalEffects CreateSignalEffects(List<Effect> signalEffects, int maxUses, int energyConsumption,
            float coolDown)
        {
            return new SignalEffects
            {
                m_effects = new List<Effect>(signalEffects),
                m_maxUses = maxUses,
                m_remainUses = maxUses,
                m_energyConsumption = energyConsumption,
                m_coolDown = new Time(coolDown),
                m_prevTriggerTime = new Time(-coolDown)
            };
        }
        
        public static SignalEffects CreateSignalEffects(List<Effect> signalEffects, int maxUses, int energyConsumption,
            Time coolDown)
        {
            return new SignalEffects
            {
                m_effects = new List<Effect>(signalEffects),
                m_maxUses = maxUses,
                m_remainUses = maxUses,
                m_energyConsumption = energyConsumption,
                m_coolDown = coolDown,
                m_prevTriggerTime = -coolDown
            };
        }

        public static SignalEffects CreateSignalEffects(SignalEffects other)
        {
            var signalEffects = new List<Effect>();
            foreach (var eff in other.m_effects)
            {
                signalEffects.Add(eff.CreateCopy());
            }


            return CreateSignalEffects(signalEffects, other.m_maxUses, other.m_energyConsumption, other.m_coolDown);
        }

        public void Trigger(EffectBlackBoard blackBoard)
        {
            var signal = blackBoard.signal;
            var time = blackBoard.time;
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

            if (signal.energy < m_energyConsumption)
            {
                Debug.Log("Energy not passed");
                return;
            }

            m_remainUses--;
            signal.ConsumeEnergy(m_energyConsumption);
            m_prevTriggerTime = time;
            
            foreach (var effect in m_effects)
            {
                effect.Trigger(blackBoard);
            }
            
        }

        public void SetModule(Module module)
        {
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
                m_requirements = requirements
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
        
        public void SetModule(Module module)
        {
            foreach(var effect in m_effects) effect.SetModule(module);
        }
    }
}