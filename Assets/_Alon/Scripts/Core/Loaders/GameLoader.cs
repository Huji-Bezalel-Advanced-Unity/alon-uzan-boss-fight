using System;
using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Alon.Scripts.Core.Loaders
{
    public class GameLoader : MonoBehaviour
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const int LoadMaxAmount = 100;
        private const float xMinPos = 2;
        private const float yMinPos = 2;
        private const float xMaxPos = -2;
        private const float yMaxPos = -2;
        
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private GameLoaderUI gameLoaderUI;

        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Dictionary<string, int> _loadersProgress = new Dictionary<string, int>
        {
            {"LoadMainScene", 20},
            {"OnMainSceneLoaded", 20},
            {"OnLoadComplete", 10},
            {"OnCoreManagerLoaded", 30},
            {"OnGameManagersLoaded", 20}
        };
        private GameObject _boss;
        
        // Start is called before the first frame update
        void Start()
        {
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
            gameLoaderUI.AddAccumulate(_loadersProgress["OnMainSceneLoaded"]);
            OnLoadComplete();
        }

        private void OnLoadComplete()
        {
            Destroy(this.gameObject);
            Destroy(gameLoaderUI.transform.root.gameObject);
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
            var bossPrefab = Resources.Load<GameObject>("Satan");
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

            var skeletonAnimation = _boss.GetComponent<SkeletonAnimation>();
            if (skeletonAnimation == null)
            {
                Debug.LogError("Failed to get SkeletonAnimation component.");
                return;
            }
            
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            
            GameManager.Instance.SetBoss(_boss); // Set the boss in the GameManager
        }

        private Vector3 GeneratePosition()
        {
            float x = Random.Range(xMinPos, xMaxPos);
            float y = Random.Range(yMinPos, yMaxPos);
            return new Vector3(x, y, 0);
        }
    }
}
