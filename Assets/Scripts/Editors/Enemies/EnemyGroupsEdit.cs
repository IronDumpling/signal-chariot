using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemyGroupsEdit: MonoBehaviour
    {

        public List<EnemyPlainGroupSpawnBlk> GetBlks()
        {
            var groupEdits = transform.GetComponentsInChildren<EnemySpawnGroupEdit>();
            var groups = new List<EnemyPlainGroupSpawnBlk>();

            foreach (var groupEdit in groupEdits)
            {
                groups.Add(groupEdit.CreateSpawnGroupBlks());
            }

            return groups;
        }
    }
}