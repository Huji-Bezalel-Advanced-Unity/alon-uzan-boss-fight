using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Alon.Scripts.Core.Managers;

namespace _Alon.Scripts.Core.Loaders
{
    public class GameLoader : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private GameLoaderUI gameLoaderUI;

        /// <summary>
        /// Private Constants
        /// </summary>
        private const int LoadMaxAmount = 100;

        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Dictionary<string, int> _loadersProgress = new Dictionary<string, int>
        {
            { "LoadMainScene", 30 },
            { "OnMainSceneLoaded", 5 },
            { "OnLoadComplete", 5 },
            { "OnCoreManagerLoaded", 10 },
            { "OnGameManagersLoaded", 5 },
            { "LoadEnemies", 30 },
            { "LoadBoss", 15 }
        };

        private GameObject _boss;
        private EnemyFactory _enemyFactory;

        // End Of Local Variables

        private void Awake()
        {
            _enemyFactory = new EnemyFactory();
        }

        private void Start()
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
            LoadGameManagers();
        }

        private void LoadGameManagers()
        {
            new GameManager(OnGameManagersLoaded);
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
            LoadEnemies();
            gameLoaderUI.AddAccumulate(_loadersProgress["OnMainSceneLoaded"]);
            OnLoadComplete();
        }

        private void OnUIFinished()
        {
            Destroy(gameObject);
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

            GameManager.Instance.SetBoss(_boss);
            if (GameManager.Instance.Boss == null)
            {
                Debug.LogError("Failed to set boss in GameManager.");
            }

            gameLoaderUI.AddAccumulate(_loadersProgress["LoadBoss"]);
        }

        private void LoadEnemies()
        {
            GameObject enemyGroup1 = _enemyFactory.CreateEnemy("EnemyGroup1");
            GameObject enemyGroup2 = _enemyFactory.CreateEnemy("EnemyGroup2");
            GameObject enemyGroup3 = _enemyFactory.CreateEnemy("EnemyGroup3");
            GameObject enemyGroup4 = _enemyFactory.CreateEnemy("EnemyGroup4");

            if (enemyGroup1 == null || enemyGroup2 == null || enemyGroup3 == null || enemyGroup4 == null)
            {
                Debug.LogError("Failed to load enemy prefab from Resources folder.");
                return;
            }

            GameManager.Instance.AddEnemy(enemyGroup1);
            GameManager.Instance.AddEnemy(enemyGroup2);
            GameManager.Instance.AddEnemy(enemyGroup3);
            GameManager.Instance.AddEnemy(enemyGroup4);

            gameLoaderUI.AddAccumulate(_loadersProgress["LoadEnemies"]);
        }
    }
}