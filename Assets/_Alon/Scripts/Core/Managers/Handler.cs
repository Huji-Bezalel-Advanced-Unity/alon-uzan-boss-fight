using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public void OnClickRamSmasher()
    {
        GameManager.Instance.SetPlayerToSpawn("RamSmasher");
    }
    
    public void OnClickIronGuardian()
    {
        GameManager.Instance.SetPlayerToSpawn("IronGuardian");
    }
    
    public void OnClickSwiftBlade()
    {
        GameManager.Instance.SetPlayerToSpawn("SwiftBlade");
    }
}
