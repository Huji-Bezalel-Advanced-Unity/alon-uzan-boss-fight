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
        /// <Header>
        /// Constants
        /// </Header>
        private const int LoadMaxAmount = 100;
        
        /// <Header>
        /// Serialized Fields
        /// </Header>
        [SerializeField] private GameLoaderUI gameLoaderUI;

        /// <Header>
        /// Public Fields
        /// </Header>

        /// <Header>
        /// Private Fields
        /// </Header>
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
            
            DontDestroyOnLoad(this.gameObject);
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
            Debug.Log("loading main scene...");
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
                gameLoaderUI.AddAccumulate(20);
                LoadMainScene();
            }
            else
            {
                Debug.LogException(new Exception("Core Managers Loading Failed."));
            }
        }
        
    }
}