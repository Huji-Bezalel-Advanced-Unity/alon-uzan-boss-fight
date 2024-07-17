using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    /// <summary>
    /// Factory class for creating enemy game objects from predefined prefabs.
    /// </summary>
    public class EnemyFactory
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Dictionary<string, GameObject> _enemiesPrefabs;

        // End Of Local Variables

        public EnemyFactory()
        {
            _enemiesPrefabs = new Dictionary<string, GameObject>
            {
                { "EnemyGroup1", Resources.Load<GameObject>("EnemyGroup1") },
                { "EnemyGroup2", Resources.Load<GameObject>("EnemyGroup2") },
                { "EnemyGroup3", Resources.Load<GameObject>("EnemyGroup3") },
                { "EnemyGroup4", Resources.Load<GameObject>("EnemyGroup4") }
            };
        }

        public GameObject CreateEnemy(string enemyType)
        {
            if (!_enemiesPrefabs.TryGetValue(enemyType, out GameObject prefab))
            {
                Debug.LogError($"Enemy type {enemyType} not found.");
                return null;
            }

            return Object.Instantiate(prefab);
        }
    }
}