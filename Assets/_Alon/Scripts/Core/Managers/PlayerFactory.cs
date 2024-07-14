
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class PlayerFactory
    {
        private readonly Dictionary<string, GameObject> PlayersPrefabs = new Dictionary<string, GameObject>
        {
            { "IronGuardian", Resources.Load<GameObject>("IronGuardian") },
            { "RamSmasher", Resources.Load<GameObject>("RamSmasher") },
            { "SwiftBlade", Resources.Load<GameObject>("SwiftBlade") }
        };
        
        public GameObject CreatePlayer(string playerType)
        {
            if (!PlayersPrefabs.TryGetValue(playerType, out var prefab))
            {
                Debug.LogError($"Player type {playerType} not found.");
                return null;
            }
            return prefab;
        }
    }
}
