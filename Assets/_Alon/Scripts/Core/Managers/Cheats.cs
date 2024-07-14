using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Alon.Scripts.Core.Managers
{
    public class Cheats : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                AddMesos();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                AddExp();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                KillAllEnemies();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        private void AddMesos()
        {
            UIManager.Instance.SetMesos(-1000);
        }
        
        private void AddExp()
        {
            UIManager.Instance.SetExp(1000);
        }
        
        private void KillAllEnemies()
        {
            GameManager.Instance.KillAllEnemies();
        }

        private void Reload()
        {
            SceneManager.LoadScene(0);
        }
    }
}