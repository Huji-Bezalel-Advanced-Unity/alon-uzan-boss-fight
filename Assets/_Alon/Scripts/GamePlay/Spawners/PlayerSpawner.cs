using System;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace _Alon.Scripts.GamePlay.Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private Vector3 _spawnPosition;
        private GameObject _bossPosition;
        private GameObject _playerPrefab;

        // End Of Local Variables

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Camera.main == null) return;
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane));
            mouseWorldPosition.z = 0;

            if (!NavMesh.SamplePosition(mouseWorldPosition, out var hit, 1.0f, NavMesh.AllAreas)) return;
            if (hit.hit)
            {
                _spawnPosition = hit.position;
                SpawnPlayer();
            }
            else
            {
                UIManager.Instance.Notify("Cannot Spawn Player Here");
            }
        }

        private void SpawnPlayer()
        {
            _playerPrefab = GameManager.Instance.GetPlayerToSpawn();
            if (_playerPrefab == null)
            {
                UIManager.Instance.Notify("Choose Your Player");
                return;
            }

            var mesosToTake = _playerPrefab.GetComponent<BasePlayerController>().GetMesosCost();
            if (UIManager.Instance.GetMesos() + mesosToTake < 0)
            {
                UIManager.Instance.Notify("Not Enough Mesos");
                return;
            }

            UIManager.Instance.SetMesos(mesosToTake);
            var newPlayer = Instantiate(_playerPrefab, _spawnPosition, Quaternion.identity);
            print(newPlayer);
            GameManager.Instance.AddPlayer(newPlayer.GetComponent<BasePlayerController>());
        }
    }
}