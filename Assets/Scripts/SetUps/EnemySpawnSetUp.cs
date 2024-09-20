using System.Collections.Generic;
using InGame.BattleFields.Enemies;
using UnityEngine;

namespace SetUps
{
    [System.Serializable]
    public class EnemySpawnSetUp
    {
        public List<EnemyPlainGroupSpawnBlk> enemySpawnGroups;
        public List<EnemyPlainWaveBlk> enemySpawnWaves;
        public List<EnemyPlainLevelBlk> enemySpawnLevels;
    }
    
    [System.Serializable]
    public class EnemyPlainLevelBlk
    {
        public List<EnemySpawnWaveBlk> waves;

        public EnemyPlainLevelBlk CreateCopy()
        {
            return new EnemyPlainLevelBlk
            {
                waves = new List<EnemySpawnWaveBlk>(waves)
            };
        }
    }
    
    [System.Serializable]
    public struct EnemySpawnWaveBlk
    {
        public int waveID;
        public List<int> moduleRewards;

        public EnemySpawnWaveBlk CreateCopy()
        {
            return new EnemySpawnWaveBlk
            {
                waveID = waveID,
                moduleRewards = new List<int>(moduleRewards)
            };
        }
    }

    [System.Serializable]
    public struct EnemyPlainWaveBlk
    {
        public List<EnemySpawnGroupBlk> groups;
        [Tooltip("结束条件1: 一波要持续多久才能结束")][Min(0)]
        public float duration;

        [Tooltip("结束条件2: 要把这些组里的所有的怪杀掉就可以结束，" +
                 "int是groups的index")] 
        public List<int> groupsMustBeat;
        
        public EnemyPlainWaveBlk CreateCopy()
        {
            var newBlk = this;
            newBlk.groups = new List<EnemySpawnGroupBlk>(this.groups);
            newBlk.groupsMustBeat = new List<int>(this.groupsMustBeat);
            return newBlk;
        }
    }
    
    [System.Serializable]
    public struct EnemySpawnGroupBlk
    {
        public SpawningRange range;
        [Tooltip("怪物组的ID")]
        public int spawnGroupID;
    }

    [System.Serializable]
    public struct EnemyPlainGroupSpawnBlk
    {
        public List<EnemySpawningBlock> enemiesPool;
        [Tooltip("每次刷怪的数量")][Min(1)]
        public int eachSpawnCount;
        [Tooltip("每次刷怪的间隔")][Min(0.01f)]
        public float spawnInterval;
        [Tooltip("当场上该组怪物数量小于这个数字时，立即刷新")][Min(0)]
        public int minEnemies;
        [Tooltip("当场上该组怪物数量大于这个数字时不在刷新更多的怪物")][Min(0)]
        public int maxEnemies;
        [Tooltip("怪物刷新位置的逻辑")]
        public SpawnLogic locationSpawnLogic;

        public EnemyPlainGroupSpawnBlk CreateCopy()
        {
            return new EnemyPlainGroupSpawnBlk
            {
                enemiesPool = new List<EnemySpawningBlock>(enemiesPool),
                eachSpawnCount = eachSpawnCount,
                spawnInterval = spawnInterval,
                minEnemies = minEnemies,
                maxEnemies = maxEnemies,
                locationSpawnLogic = locationSpawnLogic
            };
        }
    }
    
    [System.Serializable]
    public class EnemySpawningBlock
    {
        public int enemyID;
        public string enemyName;
        
        public EnemySpawningBlock()
        {
        }

        public EnemySpawningBlock(EnemySpawningBlock other)
        {
            enemyID = other.enemyID;
            enemyName = other.enemyName;
        }
    }

    [System.Serializable]
    public struct SpawningRange
    {
        [Tooltip("在一波的什么时候开始刷新这一组的怪")][Min(0)]
        public float start;
        [Tooltip("在一波的什么时候结束刷新这一组的怪")][Min(0)]
        public float end;
    }
}