using System.Collections.Generic;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Effects;
using InGame.Effects.PlacingEffectRequirements;
using InGame.Effects.TriggerRequirements;
using InGame.Views;
using SetUps;
using UnityEngine;

namespace InGame.Boards.Modules
{

    [System.Serializable]
    public struct ModulePosition
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
    
    
    public class ModuleSlot
    {
        private Module m_module;
        private ModulePosition m_position;

        public ModulePosition position => m_position;
        public Module module => m_module;

        public BoardPosition GetBoardPosition(BoardPosition pivotPos)
        {
            if (m_module == null) return default(BoardPosition);

            return m_module.GetBoardPosition(this, pivotPos);
        }
        public static ModuleSlot CreateModuleSlot(Module module, ModulePosition position)
        {
            var slot = new ModuleSlot
            {
                m_module = module,
                m_position = position
            };
            return slot;
        }

        public void TriggerEffect(EffectBlackBoard bb)
        {
            module.TriggerSignalEffect(bb);
        }

        public void Reset()
        {
            module.Reset();
        }

        public void AddBuff(ModuleBuff buff)
        {
            m_module.AddBuff(buff);
        }

        public void RemoveBuff(ModuleBuff buff)
        {
            m_module.RemoveBuff(buff);
        }
    }

    public enum Category
    {
        信号_源,
        信号_路由,
        装备_武器,
        装备_防具,
        装备_机体,
        辅助,
        默认,
    }

    public class Module
    {
        public enum Orientation
        {
            Up,
            Right,
            Down,
            Left
        }

        private Orientation m_orientation = Orientation.Up;
        private ModuleView m_prefab;
        private ModuleView m_moduleView;
        private BoardPosition m_pos = BoardPosition.invalidPosition;
        
        #region Shape
        private ModulePosition m_pivot;
        private bool[,] m_slotMap =null; 
        private ModuleSlot[,] m_moduleSlots=null;
        private int m_width, m_height;
        #endregion
        
        public string name { get; protected set; }
        public Category category { get; protected set; }
        public string leftBox { get; protected set; }
        public string rightBox { get; protected set; }
        public string desc { get; protected set; }
        public string tag { get; protected set; }
        public string function { get; protected set; }
        
        #region Effect

        private SignalEffects m_signalEffects;

        private PlacingEffects m_placingEffects;

        private CustomEffect m_customEffect;
        #endregion
        
        #region Buffs

        private ModuleBuffType m_buffMasks;

        private Dictionary<ModuleBuffType, ModuleBuff> m_buffs =
            new Dictionary<ModuleBuffType, ModuleBuff>();

        #endregion
        public ModuleView moduleView => m_moduleView;
        public Orientation orientation => m_orientation;

        #region ModuleSlot
        private void CreateSlotMap(List<ModulePosition> positions)
        {
            ModulePosition min = new ModulePosition
            {
                x = 0,
                y = 0
            };
            ModulePosition max = new ModulePosition
            {
                x = 0,
                y = 0
            };
            foreach (var pos in positions)
            {
                if (pos.x > max.x) max.x = pos.x;
                if (pos.y > max.y) max.y = pos.y;
                if (pos.x < min.x) min.x = pos.x;
                if (pos.y < min.y) min.y = pos.y;
            }

            m_width = max.x - min.x + 1;
            m_height = max.y - min.y + 1;
            
            // the default value of bool is false, so every element is false right now
            m_slotMap = new bool[m_width, m_height];
            
                
            foreach (var pos in positions)
            {
                if (pos.x > max.x) max.x = pos.x;
                if (pos.y > max.y) max.y = pos.y;
                if (pos.x < min.x) min.x = pos.x;
                if (pos.y < min.y) min.y = pos.y;
            }

            m_pivot = new ModulePosition{x = 0 - min.x, y = 0 - min.y};
            m_slotMap[m_pivot.x, m_pivot.y] = true;

            foreach (var pos in positions)
            {
                m_slotMap[pos.x - min.x, pos.y - min.y] = true;
            }
        }
        public List<ModuleSlot> GetModuleSlots()
        {
            List<ModuleSlot> slots = new List<ModuleSlot>();
            if (m_moduleSlots == null)
            {
                m_moduleSlots = new ModuleSlot[m_width, m_height];
                for (int x = 0; x < m_width; x++)
                {
                    for (int y = 0; y < m_height; y++)
                    {
                        if (m_slotMap[x, y])
                        {
                            var offset = new ModulePosition
                            {
                                x = x,
                                y = y
                            };
                            m_moduleSlots[x, y] =
                                ModuleSlot.CreateModuleSlot(this, offset);
                        }
                        else
                        {
                            m_moduleSlots[x, y] = null;
                        }
                    }
                }
            }
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    if (m_slotMap[x, y])
                    {
                        slots.Add(m_moduleSlots[x,y]);
                    }
                }
            }

            
            return slots;
        }
        #endregion
        
        #region Offset
        private ModulePosition GetOffset(int x, int y)
        {
            return GetRotatedPos(new ModulePosition
            {
                x = x - m_pivot.x,
                y = y - m_pivot.y
            }, m_orientation);
        }
        
        private ModulePosition GetOffset(ModulePosition pos)
        {
            return GetOffset(pos.x, pos.y);
        }
        
        public ModulePosition GetOffset(ModuleSlot slot)
        {
            return GetOffset(slot.position.x, slot.position.y);
        }
        #endregion
        
        #region BoardPosition

        public void SetPivotBoardPosition(BoardPosition pos)
        {
            m_pos = pos;
        }

        public BoardPosition GetPivotBoardPosition() => m_pos;
        
        
        public BoardPosition GetBoardPosition(ModuleSlot slot, BoardPosition pivotPos)
        {
            var offset = GetOffset(slot.position.x, slot.position.y);
            return new BoardPosition
            {
                x = pivotPos.x + offset.x,
                y = pivotPos.y + offset.y
            };
        }

        public BoardPosition GetPivotBoardPosition(ModuleSlot slot, BoardPosition slotBoardPosition)
        {
            var offset = GetOffset(slot);
            return new BoardPosition
            {
                x = slotBoardPosition.x - offset.x,
                y = slotBoardPosition.y - offset.y
            };
        }
        
        
        public List<BoardPosition> GetBoardPositionList(BoardPosition pivotPos)
        {
            var list = new List<BoardPosition>();
            list.Add(pivotPos);

            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    if (m_slotMap[x, y])
                    {
                        var offset = GetOffset(x,y);
                        list.Add(new BoardPosition
                        {
                            x = pivotPos.x + offset.x,
                            y = pivotPos.y + offset.y
                        });
                    }
                }
            }

            return list;
        }

        public List<BoardPosition> GetBoardPositionList()
        {
            if (m_pos != BoardPosition.invalidPosition)
            {
                return GetBoardPositionList(m_pos);
            }
            else
            {
                return new List<BoardPosition>();
            }
        }
        #endregion
        
        #region Orientation
        public void Rotate()
        {
            m_orientation = RotateClockwise(m_orientation);
        }
        
        public float GetRotationDegree()
        {
            return GetRotationDegree(m_orientation);
        }

        public void SetOrientation(Orientation newOrientation)
        {
            m_orientation = newOrientation;
        }
        #endregion
        
        #region EffectFunction

        public void TriggerSignalEffect(EffectBlackBoard bb)
        {

            m_signalEffects.Trigger(bb);
        }

        public void TriggerPlacingEffect(EffectBlackBoard bb)
        {
        
            m_placingEffects.Trigger(bb);
        }

        public void UnTriggerPlacingEffect(EffectBlackBoard bb)
        {

            m_placingEffects.UnTrigger(bb);
        }

        public void TriggerCustomEffect(RequirementBlackBoard bb)
        {
            bb.buffs = new List<ModuleBuff>();
            foreach (var effectPair in m_buffs)
            {
                bb.buffs.Add(effectPair.Value);
            }
            
            m_customEffect?.Register(bb);
        }

        public void UnTriggerCustomEffect(RequirementBlackBoard bb)
        {
            bb.buffs = new List<ModuleBuff>();
            foreach (var effectPair in m_buffs)
            {
                bb.buffs.Add(effectPair.Value);
            }
            
            m_customEffect?.Unregister(bb);
        }

        public void Reset()
        {
            // Can not reset placing effect, because it will reset when it remove it
            m_signalEffects.Reset();
            m_customEffect?.Reset();
        }
        #endregion
        
        #region BuffFunction

        private void GenerateBuffDictionary()
        {
            m_buffs.Clear();
            var list = ModuleBuff.GetBuffTypeList(m_buffMasks);

            foreach (var type in list)
            {
                m_buffs.Add(type, ModuleBuff.CreateEmptyBuff(type));
            }
        }

        public void AddBuff(ModuleBuff buff)
        {
            if (buff == null)
            {
                Debug.LogError("Null");
                return;
            }

            if (!m_buffs.ContainsKey(buff.type))
            {
                Debug.Log($"Filter {buff.type} on {name}");
                return;
            }
            
            m_buffs[buff.type].Add(buff);
            
            m_signalEffects.AddBuff(buff);
            m_placingEffects.AddBuff(buff);
            m_customEffect?.AddBuff(buff);
        }

        public void RemoveBuff(ModuleBuff buff)
        {
            if (buff == null)
            {
                Debug.LogError("Null");
                return;
            }

            if (!m_buffs.ContainsKey(buff.type))
            {
                Debug.Log($"Filter {buff.type} on {name}");
                return;
            }
            
            m_buffs[buff.type].Minus(buff);
            
            m_signalEffects.RemoveBuff(buff);
            m_placingEffects.RemoveBuff(buff);
            m_customEffect?.RemoveBuff(buff);
        }
        
        public void ClearBuffs(){
            m_signalEffects.ClearBuffs();
            m_placingEffects.ClearBuffs();
            m_customEffect?.ClearBuffs();
            
        }


        #endregion
        
        #region Animation
        public void PlayAnimation(string animationName)
        {
            m_moduleView.PlayAnimation(animationName);
        }
        #endregion
        
        #region Life Cycle

        public void SelfDestroy()
        {
            m_moduleView?.SelfDestroy();
        }
        #endregion
        
        #region static method
        public static Orientation RotateClockwise(Orientation current)
        {
            return current switch
            {
                Orientation.Up => Orientation.Right,
                Orientation.Right => Orientation.Down,
                Orientation.Down => Orientation.Left,
                Orientation.Left => Orientation.Up,
                _ => Orientation.Up
            };
        }
        
        public static Orientation RotateCounterClockwise(Orientation current)
        {
            return current switch
            {
                Orientation.Up => Orientation.Left,
                Orientation.Right => Orientation.Up,
                Orientation.Down => Orientation.Right,
                Orientation.Left => Orientation.Down,
                _ => Orientation.Up
            };
        }
        
        public static ModulePosition GetRotatedPos(ModulePosition pos, Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Up => new ModulePosition{x=pos.x, y=pos.y},
                Orientation.Down => new ModulePosition{x=-pos.x, y=-pos.y},
                Orientation.Right => new ModulePosition{x = pos.y, y= -pos.x },
                Orientation.Left => new ModulePosition{x=-pos.y, y = pos.x},
                _ => default(ModulePosition)
            };
        }
        public static float GetRotationDegree(Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Up => 0,
                Orientation.Right => -90,
                Orientation.Down => 180,
                Orientation.Left => -270,
                _=>0
            };
        }

        public static Module CreateModule(ModuleSetUp setUp)
        {
            var signalEffects = new List<Effect>();
            foreach (var eff in setUp.signalEffects)
            {
                signalEffects.Add(eff.CreateCopy());
            }

            var placingEffects = new List<Effect>();
            foreach (var eff in setUp.placingEffects)
            {
                placingEffects.Add(eff.CreateCopy());
            }

            var placingReqs = new List<PlacingEffectRequirement>();
            foreach (var req in setUp.requirements)
            {
                placingReqs.Add(req.CreateCopy());
            }

            CustomEffect customEffect = null;
            if (setUp.hasCustomEffect)
            {
                var customEffectLists = new List<Effect>();
                var triggerRequirement = setUp.triggerRequirement?.CreateCopy();

                foreach (var effect in setUp.customEffects)
                {
                    customEffectLists.Add(effect.CreateCopy());
                }
                
                customEffect = CustomEffect.CreateCustomEffect(triggerRequirement, customEffectLists);
            }
            
            var newModule = new Module
            {
                name = setUp.name,
                leftBox = setUp.leftBox,
                rightBox = setUp.rightBox,
                desc = setUp.desc,
                tag = setUp.tag,
                function = setUp.function,
                category = setUp.category,
                m_buffMasks = setUp.buffMask,
                m_prefab = setUp.prefab,
                m_signalEffects = SignalEffects.CreateSignalEffects(signalEffects, setUp.maxUses, 
                    setUp.consumptionMethod, setUp.energyConsumption, setUp.maxStoredPerTrigger, setUp.coolDown, setUp.signalMask),
                m_placingEffects = PlacingEffects.CreatePlacingEffects(placingEffects, placingReqs),
                m_customEffect = customEffect
            };
            
            
            newModule.CreateSlotMap(setUp.otherPositions);
            return newModule;
        }

        public static Module CreateModule(Module other)
        {
            
            var newModule = new Module
            {
                m_height = other.m_height,
                m_width = other.m_width,
                m_orientation = Orientation.Up,
                m_pivot = other.m_pivot,
                m_prefab = other.m_prefab,
                m_slotMap = other.m_slotMap,
                name = other.name,
                leftBox = other.leftBox,
                rightBox = other.rightBox,
                desc = other.desc,
                tag = other.tag,
                function = other.function,
                category = other.category,
                m_buffMasks = other.m_buffMasks,
                m_signalEffects = SignalEffects.CreateSignalEffects(other.m_signalEffects),
                m_placingEffects = PlacingEffects.CreatePlacingEffects(other.m_placingEffects),
                m_customEffect = CustomEffect.CreateCustomEffect(other.m_customEffect)
            };
            newModule.m_signalEffects.SetModule(newModule);
            newModule.m_placingEffects.SetModule(newModule);
            newModule.m_customEffect?.SetModule(newModule);
            
            newModule.GenerateBuffDictionary();
            return newModule;
        }

        public ModuleView CreateModuleView(Transform parent)
        {
            if (m_moduleView != null) return m_moduleView;
            if (m_prefab == null) return null;

            m_moduleView = ModuleView.CreateModuleView(m_prefab, parent, this,
                Quaternion.Euler(0, 0, GetRotationDegree(m_orientation)));
            return m_moduleView;
        }
        
        #endregion
        
        #region ViewFunctionRedirection
        public void DisplayProgressBar(float current, float max)
        {
            m_moduleView?.DisplayUI(current, max);
        }
        #endregion

        public static BoardPosition OrientationToBoardPositionOffset(Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Up => new BoardPosition(0, 1),
                Orientation.Down => new BoardPosition(0, -1),
                Orientation.Right => new BoardPosition(1, 0),
                Orientation.Left => new BoardPosition(-1, 0),
                _ => new BoardPosition(0, 0) 
            };
        }
        public override string ToString()
        {
            string result = "";
            result = $"{name}: ";
            result += $"slot Map size: {m_width}, {m_height}, ";
            result += $"pivot: {m_pivot}, ";
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    result += $"({x}, {y}): {m_slotMap[x, y]}, ";
                }
            }
            
            
            return result;
        }
    }
}