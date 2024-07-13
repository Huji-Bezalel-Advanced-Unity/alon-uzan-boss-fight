
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class PlayerFactory
    {
        public static GameObject CreatePlayer(string playerType)
        {
            if (!GameManager.Instance.PlayersPrefabs.TryGetValue(playerType, out var prefab))
            {
                Debug.LogError($"Player type {playerType} not found.");
                return null;
            }
            return Object.Instantiate(prefab);
        }
    }
}
