using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class CoreManager
    {
        /// <Header>
        /// Constants
        /// </Header>

        /// <Header>
        /// Serialized Fields
        /// </Header>

        /// <Header>
        /// Public Fields
        /// </Header>

        /// <Header>
        /// Private Fields
        /// </Header>
        private Action<bool> _onComplete;

        public CoreManager(Action<bool> onComplete)
        {
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
    }
    

}
