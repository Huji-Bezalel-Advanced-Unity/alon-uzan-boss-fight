using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

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
    
    public void OnUpgradeRamSmasher(Button button)
    {
        if (UIManager.Instance.GetExp() < 2500)
        {
            // TODO: Show message
            UIManager.Instance.Notify("Not enough exp");
            Debug.Log("Not enough exp");
            return;
        }
        UIManager.Instance.SetExp(-2500);
        GameManager.Instance.UpgradePlayer("RamSmasher");
        var block = button.colors;
        block.normalColor = Color.yellow;
        block.highlightedColor = Color.yellow;
        block.pressedColor = Color.yellow;
        block.selectedColor = Color.yellow;
        button.colors = block;
        AudioManager.Instance.PlayAudioClip(3);
    }
    
    public void OnUpgradeIronGuardian(Button button)
    {
        if (UIManager.Instance.GetExp() < 1000)
        {
            // TODO: Show message
            UIManager.Instance.Notify("Not enough exp");
            Debug.Log("Not enough exp");
            return;
        }
        UIManager.Instance.SetExp(-1000);
        GameManager.Instance.UpgradePlayer("IronGuardian");
        var block = button.colors;
        block.normalColor = Color.yellow;
        block.highlightedColor = Color.yellow;
        block.pressedColor = Color.yellow;
        block.selectedColor = Color.yellow;
        button.colors = block;
        AudioManager.Instance.PlayAudioClip(3);

    }
    
    public void OnUpgradeSwiftBlade(Button button)
    {
        if (UIManager.Instance.GetExp() < 1500)
        {
            // TODO: Show message
            UIManager.Instance.Notify("Not enough exp");
            Debug.Log("Not enough exp");
            return;
        }
        UIManager.Instance.SetExp(-1500);
        GameManager.Instance.UpgradePlayer("SwiftBlade");
        var block = button.colors;
        block.normalColor = Color.yellow;
        block.highlightedColor = Color.yellow;
        block.pressedColor = Color.yellow;
        block.selectedColor = Color.yellow;
        button.colors = block;
        AudioManager.Instance.PlayAudioClip(3);
    }
}