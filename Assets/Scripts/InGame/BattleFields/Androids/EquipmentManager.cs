using System.Collections.Generic;

using SetUps;
using InGame.Boards.Modules;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.BattleFields.Bullets;
using UnityEngine;

namespace InGame.BattleFields.Androids
{
    public class EquipmentManager
    {
        private List<Equipment> m_equipments;

        public EquipmentManager()
        {
            m_equipments = new();
        }

        public List<Equipment> GetEquipments() => m_equipments;
        
        public void CopyEquipments(List<Equipment> equipments)
        {
            m_equipments = equipments;
        }
        
        public Equipment AddEquipment(EquipmentSetUp equipmentSetUp, Module module)
        {
            Equipment equipment = new(equipmentSetUp, module);
            m_equipments.Add(equipment);
            return equipment;
        }
        
        public void RemoveEquipment(Equipment equipment)
        {
            m_equipments.Remove(equipment);
            equipment.Die();
        }

        public void ClearEquipment()
        {
            foreach(Equipment equipment in m_equipments)
            {
                equipment.Die();
            }
            m_equipments.Clear();
        }

        public void EquipmentEffect(Module module, BulletType type, int level, WeaponBuff buff)
        {
            foreach(Equipment equipment in m_equipments)
            {
                if(module == equipment.module)
                    equipment.Effect(buff, type, level);
            }
        }
    }
}