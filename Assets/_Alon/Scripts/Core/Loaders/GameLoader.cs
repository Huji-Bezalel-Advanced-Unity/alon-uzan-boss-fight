using System;
using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Alon.Scripts.Core.Loaders
{
    public class GameLoader : MonoBehaviour
    {
        private const int LoadMaxAmount = 100;
        private const float XMinPos = -10;
        private const float YMinPos = 5.5f;
        private const float XMaxPos = -4;
        private const float YMaxPos = 1.5f;

        [SerializeField] private GameLoaderUI gameLoaderUI;
        private readonly Dictionary<string, int> _loadersProgress = new Dictionary<string, int>
        {
            { "LoadMainScene", 20 },
            { "OnMainSceneLoaded", 20 },
            { "OnLoadComplete", 10 },
            { "OnCoreManagerLoaded", 30 },
            { "OnGameManagersLoaded", 20 }
        };

        private GameObject _boss;
        private GameObject _enemy;

        void Start()
        {
            gameLoaderUI.OnUIFinished += OnUIFinished;
            StartCoroutine(StartLoadingAsync());
        }

        private IEnumerator StartLoadingAsync()
        {
            yield return new WaitForSeconds(0.1f);

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gameLoaderUI.transform.root.gameObject);

            gameLoaderUI.Init(LoadMaxAmount);

            LoadCoreManagers();
            LoadGameManagers();
        }

        private void LoadCoreManagers()
        {
            var coreManager = new CoreManager(OnCoreManagerLoaded);
        }

        private void LoadGameManagers()
        {
            var gameManager = new GameManager(OnGameManagersLoaded);
        }

        private void OnGameManagersLoaded(bool isSuccess)
        {
            if (isSuccess)
            {
                gameLoaderUI.AddAccumulate(_loadersProgress["OnGameManagersLoaded"]);
                LoadMainScene();
            }
            else
            {
                Debug.LogException(new Exception("Core Managers Loading Failed."));
            }
        }

        private void LoadMainScene()
        {
            SceneManager.sceneLoaded += OnMainSceneLoaded;
            SceneManager.LoadScene("BossMain");
            gameLoaderUI.AddAccumulate(_loadersProgress["LoadMainScene"]);
        }

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            LoadBoss();
            LoadEnemy();
            gameLoaderUI.AddAccumulate(_loadersProgress["OnMainSceneLoaded"]);
            OnLoadComplete();
        }

        private void OnUIFinished()
        {
            Destroy(this.gameObject);
            Destroy(gameLoaderUI.transform.root.gameObject);
        }

        private void OnLoadComplete()
        {
            gameLoaderUI.AddAccumulate(_loadersProgress["OnLoadComplete"]);
        }

        private void OnCoreManagerLoaded(bool isSuccess)
        {
            if (isSuccess)
            {
                gameLoaderUI.AddAccumulate(_loadersProgress["OnCoreManagerLoaded"]);
            }
            else
            {
                Debug.LogException(new Exception("Core Managers Loading Failed."));
            }
        }

        private void LoadBoss()
        {
            var bossPrefab = Resources.Load<GameObject>("Mage");
            if (bossPrefab == null)
            {
                Debug.LogError("Failed to load boss prefab from Resources folder.");
                return;
            }

            Vector3 spawnPosition = GeneratePosition();
            _boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            if (_boss == null)
            {
                Debug.LogError("Boss instantiation failed.");
                return;
            }


            GameManager.Instance.SetBoss(_boss); // Set the boss in the GameManager

            if (GameManager.Instance.Boss == null)
            {
                Debug.LogError("Failed to set boss in GameManager.");
            }
            
            
        }
        
        private void LoadEnemy()
        {
            GameObject enemyPrefab = Resources.Load<GameObject>("EnemyGroup1");
            if (enemyPrefab == null)
            {
                Debug.LogError("Failed to load enemy prefab from Resources folder.");
                return;
            }
            _enemy = Instantiate(enemyPrefab, new Vector3(11, 0, 0), Quaternion.identity);
            if (_enemy == null)
            {
                Debug.LogError("Enemy instantiation failed.");
                return;
            }

            foreach (Transform child in _enemy.transform)
            {
                GameManager.Instance.AddEnemy(child.gameObject);
            }
        }

        private Vector3 GeneratePosition()
        {
            float x = UnityEngine.Random.Range(XMinPos, XMaxPos);
            float y = UnityEngine.Random.Range(YMinPos, YMaxPos);
            return new Vector3(x, y, 0);
        }
    }
}
