using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SetUps
{
    [CreateAssetMenu(fileName = "Set Up", menuName = "Set Up", order = 0)]
    public class SetUp: ScriptableObject
    {
        public ChariotSetUp chariotSetUp;
        public BoardSetUp boardSetUp;
        public BoardSetUp extraBoardSetUp;
        public List<ModuleSetUp> moduleLibrary;
        public List<EnemySetUp> enemyLibrary;
        public List<EnemyPlainGroupSpawnBlk> enemySpawnGroups;
        public List<EnemyPlainWaveBlk> enemySpawnWaves;
        public List<EnemyPlainLevelBlk> enemySpawnLevels;
    }
}