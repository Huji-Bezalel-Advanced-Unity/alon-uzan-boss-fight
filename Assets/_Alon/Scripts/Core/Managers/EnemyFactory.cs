
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class EnemyFactory
    {
        private readonly Dictionary<string, GameObject> EnemiesPrefabs = new Dictionary<string, GameObject>
        {
            { "EnemyGroup1", Resources.Load<GameObject>("EnemyGroup1") },
            { "EnemyGroup2", Resources.Load<GameObject>("EnemyGroup2") },
            { "EnemyGroup3", Resources.Load<GameObject>("EnemyGroup3") },
            { "EnemyGroup4", Resources.Load<GameObject>("EnemyGroup4") }
        };
        
        public GameObject CreateEnemy(string enemyType)
        {
            // Assuming a similar dictionary for enemies in GameManager
            if (!EnemiesPrefabs.ContainsKey(enemyType))
            {
                Debug.LogError($"Enemy type {enemyType} not found.");
                return null;
            }
            return Object.Instantiate(EnemiesPrefabs[enemyType]);
        }
    }
}
