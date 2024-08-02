using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemySpawnLevelEdit: MonoBehaviour
    {
        public List<EnemySpawnWaveBlk> waves; 
        public EnemyPlainLevelBlk CreteBlk()
        {
            
            var level = new EnemyPlainLevelBlk
            {
                waves = new List<EnemySpawnWaveBlk>()
            };
            foreach (var blk in waves)
            {
                level.waves.Add(blk);
            }

            return level;
        }
    }
}