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
        /// Constants
        /// </summary>
        private const int LoadMaxAmount = 100;
        
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private GameLoaderUI gameLoaderUI;

        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Dictionary<string, int> _loadersProgress = new Dictionary<string, int>
        {
            {"LoadMainScene", 50}
        };
        
        
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
        }
        
        private void LoadCoreManagers()
        {
            var coreManager = new CoreManager(OnCoreManagerLoaded);
        }

        private void LoadMainScene()
        {
            SceneManager.LoadScene("BossMain");
            gameLoaderUI.AddAccumulate(_loadersProgress["LoadMainScene"]);
            SceneManager.sceneLoaded += OnMainSceneLoaded;
        }

        private void OnMainSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnMainSceneLoaded;

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
                gameLoaderUI.AddAccumulate(50);
                LoadMainScene();
            }
            else
            {
                Debug.LogException(new Exception("Core Managers Loading Failed."));
            }
        }
        
    }
}