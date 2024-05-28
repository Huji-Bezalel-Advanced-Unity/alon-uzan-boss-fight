using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class CoreManager
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Action<bool> _onComplete;
        public static CoreManager instance;
        
        public CoreManager(Action<bool> onComplete)
        {
            if (instance != null)
            {
                Debug.LogException(new Exception("CoreManager already exists"));
                return;
            }
            
            instance = this;
            
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
