using System.Collections.Generic;
using SetUps;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editors.Enemies
{
    
    public class EnemySpawnGroupEdit: MonoBehaviour
    {
        public List<EnemySpawningBlock> enemies;
        public void OnValidate()
        {

            var enemyLib = EnemyRoot.Instance.setUp.enemyLibrary;

            if (enemies == null) return;
            foreach (var blk in enemies)
            {
                
                if (blk.enemyID >= 0 && blk.enemyID < enemyLib.Count)
                {
                    blk.enemyName = enemyLib[blk.enemyID].name;
                }
                else
                {
                    blk.enemyName = "Invalid ID";
                }

            }
            
            

        }

        public EnemyPlainGroupSpawnBlk CreateSpawnGroupBlks()
        {
            var newList = new List<EnemySpawningBlock>();

            foreach (var blk in enemies)
            {
                newList.Add(new EnemySpawningBlock(blk));
            }

            return new EnemyPlainGroupSpawnBlk{enemies = newList};
        }
    }
}