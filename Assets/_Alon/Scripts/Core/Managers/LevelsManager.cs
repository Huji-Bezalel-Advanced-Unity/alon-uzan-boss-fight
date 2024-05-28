using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class LevelsManager
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Action<bool> _onComplete;
        private List<LevelsData> _levelsData;
        
        public LevelsManager(Action<bool> onComplete)
        {
            this._onComplete = onComplete;
            CreateLevels();
            OnLoadSuccess();
        }

        private void CreateLevels()
        {
            _levelsData = new List<LevelsData>
            {
                new LevelsData {Name = "Boss Level", SceneName = "BossMain", BossName = "Warrok", Status = true},
            };
        }

        private void OnLoadFail()
        {
            _onComplete?.Invoke(false);
        }
        
        private void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }
        
        public LevelsData GetLevelData(string levelName)
        {
            return _levelsData.Find(x => x.Name == levelName);
        }
        
        public LevelsData GetLevelData(int index)
        {
            return _levelsData[index];
        }
        
        public struct LevelsData
        {
            public string Name;
            public string SceneName;
            public string BossName;
            public bool Status;
        }
        
    }
}