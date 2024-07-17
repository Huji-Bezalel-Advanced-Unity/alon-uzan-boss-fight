using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class PlayerFactory
    {
        /// <summary>
        /// private Fields
        /// </summary>
        private readonly Dictionary<string, GameObject> _playersPrefabs = new Dictionary<string, GameObject>
        {
            { "IronGuardian", Resources.Load<GameObject>("IronGuardian") },
            { "RamSmasher", Resources.Load<GameObject>("RamSmasher") },
            { "SwiftBlade", Resources.Load<GameObject>("SwiftBlade") }
        };

        // End Of Local Variables

        public GameObject CreatePlayer(string playerType)
        {
            if (_playersPrefabs.TryGetValue(playerType, out var prefab)) return prefab;
            Debug.LogError($"Player type {playerType} not found.");
            return null;

        }
    }
}