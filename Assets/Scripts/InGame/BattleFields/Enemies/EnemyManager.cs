

using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace InGame.BattleFields.Enemies
{
    public class EnemyManager
    {
        private readonly List<EnemyPlainLevelBlk> m_spawnSetUps;
        private int m_currentLevel;
        private int m_currentWave;

        private float m_duration;
        private List<EnemyGroup> m_groups = new();
        
        public EnemyManager(List<EnemyPlainLevelBlk> spawnSetUp)
        {
            m_spawnSetUps = spawnSetUp;
            m_currentLevel = 0;
            m_currentWave = 0;
        }

        private EnemySpawnWaveBlk GetWaveBlk(int level, int wave)
        {
            if (level < 0 || level >= m_spawnSetUps.Count)
            {
                Debug.LogError("Level out of index");
                return default;
            }

            var waves = m_spawnSetUps[level].waves;
            
            if (wave < 0 || wave >= waves.Count)
            {
                Debug.LogError("Wave out of index");
                return default;
            }

            return waves[wave];

        }
        public void SetWave(int level, int wave)
        {
            m_currentLevel = level;
            m_currentWave = wave;
        }

        public void Prepare()
        {
            var waveBlk = GetWaveBlk(m_currentLevel, m_currentWave);
            m_duration = waveBlk.duration;
            m_groups.Clear();
            
            
        }

        
    }
}