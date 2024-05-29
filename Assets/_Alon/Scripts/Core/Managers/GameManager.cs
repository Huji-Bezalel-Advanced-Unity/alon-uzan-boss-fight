using System;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class GameManager
    {
        private readonly Action<bool> _onComplete;
        public static GameManager Instance { get; private set; }

        public GameObject Boss { get; private set; }

        public GameManager(Action<bool> onComplete)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogException(new Exception("GameManager already exists"));
                return;
            }

            _onComplete = onComplete;
            OnLoadSuccess();
        }
        
        private void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }

        public void OnLoadFail()
        {
            _onComplete?.Invoke(false);
        }

        public void SetBoss(GameObject boss)
        {
            Boss = boss;
        }
    }
}