using System.Collections.Generic;
using SetUps;
using UnityEngine;

namespace Editors.Enemies
{
    public class EnemyWavesEdit: MonoBehaviour
    {
        public List<EnemyPlainWaveBlk> GetBlks()
        {
            var groupEdits = transform.GetComponentsInChildren<EnemySpawnWaveEdit>();
            var waves = new List<EnemyPlainWaveBlk>();

            foreach (var groupEdit in groupEdits)
            {
                waves.Add(groupEdit.CreateBlk());
            }

            return waves;
        }
    }
}