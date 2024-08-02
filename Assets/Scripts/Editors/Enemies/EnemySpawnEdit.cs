using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemySpawnEdit: MonoBehaviour
    {
        public List<EnemyPlainLevelBlk> CreateSetUp()
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