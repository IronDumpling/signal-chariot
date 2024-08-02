using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace InGame.BattleFields.Enemies
{
    public class EnemyLib
    {
        public List<Enemy> m_enemies = new();

        public EnemyLib(List<EnemySetUp> setUps)
        {
            for(int i = 0; i < setUps.Count; i++)
            {
                Debug.Log(i);
                var setUp = setUps[i];
                m_enemies.Add(Enemy.CreateEnemy(setUp, i));
            }
        }

        public Enemy CreateEnemy(int id)
        {
            if (id < 0 || id >= m_enemies.Count) return null;

            return Enemy.CreateEnemy(m_enemies[id]);
        }
    }
}