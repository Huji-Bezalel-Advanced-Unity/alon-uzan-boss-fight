using System;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.EventSystems; // Import the EventSystems namespace
using UnityEngine.AI; // Import the AI namespace

namespace _Alon.Scripts.GamePlay.Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private Vector3 _spawnPosition;

        private GameObject _bossPosition;
        private GameObject playerPrefab;

        // End Of Local Variables

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the pointer is over a UI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // Convert mouse position to world position
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, Camera.main.nearClipPlane));
                    mouseWorldPosition.z =
                        0; // Set Z to 0 or other appropriate value depending on your game's coordinate system

                    // Check if the world position is on a walkable NavMesh area
                    if (NavMesh.SamplePosition(mouseWorldPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    {
                        if (hit.hit)
                        {
                            _spawnPosition = hit.position;
                            Debug.Log("Spawn position set at a walkable NavMesh area: " + _spawnPosition);
                            SpawnPlayer();
                        }
                        else
                        {
                            Debug.Log("Clicked position is not on a walkable NavMesh area.");
                        }
                    }
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
            var mesosToTake = playerPrefab.GetComponent<BasePlayerController>().GetMesosCost();
            if (UIManager.Instance.GetMesos() - mesosToTake <= 0)
            {
                return;
            }

            UIManager.Instance.SetMesos(mesosToTake);
            GameObject newPlayer = Instantiate(playerPrefab, _spawnPosition, Quaternion.identity);
            print(newPlayer);
            GameManager.Instance.AddPlayer(newPlayer.GetComponent<BasePlayerController>());
        }
    }
}