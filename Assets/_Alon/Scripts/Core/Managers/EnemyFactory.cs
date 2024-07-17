using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class EnemyFactory
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Dictionary<string, GameObject> _enemiesPrefabs = new()
        {
            { "EnemyGroup1", Resources.Load<GameObject>("EnemyGroup1") },
            { "EnemyGroup2", Resources.Load<GameObject>("EnemyGroup2") },
            { "EnemyGroup3", Resources.Load<GameObject>("EnemyGroup3") },
            { "EnemyGroup4", Resources.Load<GameObject>("EnemyGroup4") }
        };

        // End Of Local Variables

        public GameObject CreateEnemy(string enemyType)
        {
            if (_enemiesPrefabs.TryGetValue(enemyType, out var prefab)) return Object.Instantiate(prefab);
            Debug.LogError($"Enemy type {enemyType} not found.");
            return null;

        }
    }
}