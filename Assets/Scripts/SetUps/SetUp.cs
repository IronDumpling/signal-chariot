using System.Collections.Generic;
using UnityEngine;

namespace SetUps
{
    [CreateAssetMenu(fileName = "Set Up", menuName = "Set Up", order = 0)]
    public class SetUp: ScriptableObject
    {
        #region Android
        public AndroidSetUp androidSetUp;
        public List<EquipmentSetUp> equipmentLibrary;
        public List<BulletSetUp> bulletLibrary;
        #endregion
        
        #region Board
        public BoardSetUp boardSetUp;
        public BoardSetUp extraBoardSetUp;
        public List<ModuleSetUp> moduleLibrary;
        #endregion

        #region Enemy
        public List<EnemySetUp> enemyLibrary;
        public EnemySpawnSetUp enemySpawnSetUp;
        #endregion

        #region Audio
        public List<AudioSetUp> audioLibrary;
        #endregion
    }
}