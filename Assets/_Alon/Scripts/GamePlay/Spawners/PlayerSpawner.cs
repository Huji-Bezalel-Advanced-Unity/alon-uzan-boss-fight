using System;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.EventSystems; // Import the EventSystems namespace

namespace _Alon.Scripts.GamePlay.Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        private GameObject playerPrefab;

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
                // Check if the pointer is over a UI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _spawnPosition.z = 0;

                    SpawnPlayer();
                }
            }
        }

        private void SpawnPlayer()
        {
            playerPrefab = GameManager.Instance.GetPlayerToSpawn();
            if (playerPrefab == null)
            {
                Debug.Log("Choose Player");
                return;
            }
            GameObject newPlayer = Instantiate(playerPrefab, _spawnPosition, Quaternion.identity);
            print(newPlayer);
            GameManager.Instance.AddPlayer(newPlayer.GetComponent<BasePlayerController>());
        }
    }
}