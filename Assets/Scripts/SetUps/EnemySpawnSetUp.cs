using System.Collections.Generic;
using InGame.BattleFields.Enemies;

namespace SetUps
{
    [System.Serializable]
    public class EnemyPlainLevelBlk
    {
        public List<EnemySpawnWaveBlk> waves;
    }
    
    [System.Serializable]
    public struct EnemySpawnWaveBlk
    {
        public float duration;
        public int waveID;
    }

    [System.Serializable]
    public struct EnemyPlainWaveBlk
    {
        public List<EnemySpawnGroupBlk> groups;
    }
    
    [System.Serializable]
    public struct EnemySpawnGroupBlk
    {
        public SpawningRange range;
        public int spawnGroupID;
    }

    [System.Serializable]
    public struct EnemyPlainGroupSpawnBlk
    {
        public List<EnemySpawningBlock> enemies;
    }
    
    [System.Serializable]
    public class EnemySpawningBlock
    {
        public int enemyID;
        public string enemyName;
        public int count;
        public SpawnLogic spawnLogic;

        public EnemySpawningBlock()
        {
        }

        public EnemySpawningBlock(EnemySpawningBlock other)
        {
            enemyID = other.enemyID;
            enemyName = other.enemyName;
            count = other.count;
            spawnLogic = other.spawnLogic;
        }
    }

    [System.Serializable]
    public struct SpawningRange
    {
        public float start;
        public float end;
    }
}