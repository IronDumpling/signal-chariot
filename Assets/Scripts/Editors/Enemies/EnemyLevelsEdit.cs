using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemyLevelsEdit: MonoBehaviour
    {
        public List<EnemyPlainLevelBlk> GetBlks()
        {
            var levelEdits = transform.GetComponentsInChildren<EnemySpawnLevelEdit>();
            var levels = new List<EnemyPlainLevelBlk>();

            foreach (var levelEdit in levelEdits)
            {
                levels.Add(levelEdit.CreteBlk());
            }

            return levels;
        }
    }
}