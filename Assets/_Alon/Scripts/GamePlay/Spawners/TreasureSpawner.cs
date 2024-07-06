using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.GamePlay.Spawners
{
    public class TreasureSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPositions;

        private GameObject _treasure;

        private int _treasureCounter = 0;

        private float _timeToSpawn;

        private HashSet<int> _posMemo = new HashSet<int>();
        [SerializeField] private float timeIntervalToSpawn = 5f;

        // Start is called before the first frame update
        void Start()
        {
            _treasure = Resources.Load<GameObject>("Treasure");
        }
    
        // Update is called once per frame
        void Update()
        {
            _timeToSpawn += Time.deltaTime;
            if (_timeToSpawn >= timeIntervalToSpawn && _treasureCounter < 5)
            {
                Debug.Log("spawn treasure");
                int randPos = Random.Range(0, 10);
                while (_posMemo.Contains(randPos))
                {
                    randPos = Random.Range(0, 10);
                }

                _posMemo.Add(randPos);
                _treasureCounter++;
                Instantiate(_treasure, spawnPositions[randPos], spawnPositions[randPos]);
                _timeToSpawn = 0;
            }
        }
    }
}

