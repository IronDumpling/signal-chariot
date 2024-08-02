#if UNITY_EDITOR
using SetUps;
using UnityEditor;
using UnityEngine;
using Utils.Common;

namespace Editors.Enemies
{
    public class EnemyRoot: MonoSingleton<EnemyRoot>
    {
        public SetUp setUp;
        
        [ContextMenu("Save Asset")]
        public void SaveAssets()
        {
            setUp.enemyLibrary.Clear();
            var enemyLib = transform.Find("EnemyLib");
            var enemyEdits = enemyLib.GetComponentsInChildren<EnemyEdit>();

            foreach (var edit in enemyEdits)
            {
                setUp.enemyLibrary.Add(edit.CreateEnemySetUp());
            }
            
            setUp.enemySpawnGroups.Clear();
            var groupEdit= transform.GetComponentInChildren<EnemyGroupsEdit>();
            var list = groupEdit.GetBlks();
            foreach (var group in list)
            {
                setUp.enemySpawnGroups.Add(group);
            }
            
            setUp.enemySpawnWaves.Clear();
            var waveEdit= transform.GetComponentInChildren<EnemyWavesEdit>();
            var waveList = waveEdit.GetBlks();
            foreach (var wave in waveList)
            {
                setUp.enemySpawnWaves.Add(wave);
            }
            
            setUp.enemySpawnLevels.Clear();
            var levelEdit= transform.GetComponentInChildren<EnemyLevelsEdit>();
            var levelList = levelEdit.GetBlks();
            foreach (var level in levelList)
            {
                setUp.enemySpawnLevels.Add(level);
            }

            EditorUtility.SetDirty(setUp);
            Debug.Log("Save Asset");

        }
    }
}

#endif