using System.Collections.Generic;

using SetUps;
using InGame.Boards.Modules;

namespace InGame.BattleFields.Chariots
{
    public class TowerManager
    {
        private List<Tower> m_towers;

        public TowerManager()
        {
            m_towers = new();
        }

        public List<Tower> GetTowers() => m_towers;
        
        public void CopyTowers(List<Tower> towers)
        {
            m_towers = towers;
        }
        public Tower AddTower(TowerSetUp towerSetUp, Module module)
        {
            Tower tower = new(towerSetUp, module);
            m_towers.Add(tower);
            return tower;
        }
        public void RemoveTower(Tower tower)
        {
            m_towers.Remove(tower);
            tower.Die();
        }
        public void TowerEffect(Module module)
        {
            foreach(Tower tower in m_towers)
            {
                if(module == tower.module)
                    tower.Effect();
            }
        }
    }
}