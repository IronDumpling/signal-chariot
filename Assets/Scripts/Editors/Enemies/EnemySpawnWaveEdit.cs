using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemySpawnWaveEdit: MonoBehaviour
    {

        public List<EnemySpawnGroupBlk> groups;

        public EnemyPlainWaveBlk CreateBlk()
        {
            var blk = new EnemyPlainWaveBlk
            {
                groups = new List<EnemySpawnGroupBlk>()
            };
            foreach (var group in groups)
            {
                blk.groups.Add(group);
            }

            return blk;
        }
    }
}