
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class EnemyFactory
    {
        public static GameObject CreateEnemy(string enemyType)
        {
            // Assuming a similar dictionary for enemies in GameManager
            if (!GameManager.Instance.EnemiesPrefabs.ContainsKey(enemyType))
            {
                Debug.LogError($"Enemy type {enemyType} not found.");
                return null;
            }
            return Object.Instantiate(GameManager.Instance.EnemiesPrefabs[enemyType]);
        }
    }
}
