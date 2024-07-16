using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadGame : MonoBehaviour
{
    private bool _canReload = false;

    private void Awake()
    {
        StartCoroutine(DelayReloadingForUI());
    }

    private IEnumerator DelayReloadingForUI()
    {
        yield return new WaitForSeconds(3f);
        _canReload = true;
    }
    
    public void Reload()
    {
        if (_canReload)
        {
            SceneManager.LoadScene("Loader");
        }
    }
}
