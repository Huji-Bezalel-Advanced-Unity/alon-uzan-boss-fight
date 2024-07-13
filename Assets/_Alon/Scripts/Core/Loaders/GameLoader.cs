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
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private GameLoaderUI gameLoaderUI;

        /// <summary>
        /// Private Fields
        /// </summary>
        private const int LoadMaxAmount = 100;

        private readonly Dictionary<string, int> _loadersProgress = new Dictionary<string, int>
        {
            { "LoadMainScene", 20 },
            { "OnMainSceneLoaded", 20 },
            { "OnLoadComplete", 10 },
            { "OnCoreManagerLoaded", 30 },
            { "OnGameManagersLoaded", 20 }
        };

        private GameObject _boss;

        // End Of Local Variables

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

            _boss = Instantiate(bossPrefab, new Vector3(1.24f, 31.8f, 0), Quaternion.identity);
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
            GameObject enemyGroup1 = EnemyFactory.CreateEnemy("EnemyGroup1");
            GameObject enemyGroup2 = EnemyFactory.CreateEnemy("EnemyGroup2");
            GameObject enemyGroup3 = EnemyFactory.CreateEnemy("EnemyGroup3");
            GameObject enemyGroup4 = EnemyFactory.CreateEnemy("EnemyGroup4");
            if (enemyGroup1 == null || enemyGroup2 == null || enemyGroup3 == null || enemyGroup4 == null)
            {
                Debug.LogError("Failed to load enemy prefab from Resources folder.");
                return;
            }
            
            GameManager.Instance.AddEnemy(enemyGroup1);
            GameManager.Instance.AddEnemy(enemyGroup2);
            GameManager.Instance.AddEnemy(enemyGroup3);
            GameManager.Instance.AddEnemy(enemyGroup4);

        }
        
    }
}