using System;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class CoreManager
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Action<bool> _onComplete;

        /// <summary>
        /// Public Fields
        /// </summary>
        public static CoreManager Instance;

        // End Of Local Variables

        public CoreManager(Action<bool> onComplete)
        {
            if (Instance != null)
            {
                Debug.LogException(new Exception("CoreManager already exists"));
                return;
            }

            Instance = this;


            this._onComplete = onComplete;
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
    }
}