using System;
using UnityEngine;

namespace _Alon.Scripts.GamePlay.Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private GameObject playerPrefab;

        /// <summary>
        /// Private Fields
        /// </summary>
        private Vector3 _spawnPosition;

        private GameObject _bossPosition;


        // End Of Local Variables

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _spawnPosition.z = 0;

                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            Instantiate(playerPrefab, _spawnPosition, Quaternion.identity);
        }
    }
}