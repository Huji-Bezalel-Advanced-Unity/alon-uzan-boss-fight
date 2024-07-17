using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Managers
{
    public class Handler : MonoBehaviour
    {
        /// <summary>
        /// Private fields
        /// </summary>
        private readonly Color _goldColor = new Color(1f, 0.84f, 0f);

        [Tooltip("first is RamSmasher, second is IronGuardian, third is SwiftBlade")] [SerializeField]
        private Button[] spawnersButtons;

        [Tooltip("first is RamSmasher, second is IronGuardian, third is SwiftBlade")] [SerializeField]
        private Button[] upgradesButtons;

        // End Of Local Variables
        
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

        public void OnUpgradeRamSmasher()
        {
            if (CheckForEnoughExp(2500)) return;
            UIManager.Instance.SetExp(-2500);
            GameManager.Instance.UpgradePlayer("RamSmasher");
            SetButtonColor(spawnersButtons[0], _goldColor);
            upgradesButtons[0].interactable = false; // Disable the upgrade button
            AudioManager.Instance.PlayAudioClip(1);
        }

        private bool CheckForEnoughExp(float expNeeded)
        {
            if (UIManager.Instance.GetExp() < expNeeded)
            {
                UIManager.Instance.Notify("You Need " + expNeeded + " Exp For Upgrade");
                return true;
            }

            return false;
        }

        public void OnUpgradeIronGuardian()
        {
            if (CheckForEnoughExp(1000)) return;
            UIManager.Instance.SetExp(-1000);
            GameManager.Instance.UpgradePlayer("IronGuardian");
            SetButtonColor(spawnersButtons[1], _goldColor);
            upgradesButtons[1].interactable = false; // Disable the upgrade button
            AudioManager.Instance.PlayAudioClip(1);
        }

        public void OnUpgradeSwiftBlade()
        {
            if (CheckForEnoughExp(1500)) return;
            UIManager.Instance.SetExp(-1500);
            GameManager.Instance.UpgradePlayer("SwiftBlade");
            SetButtonColor(spawnersButtons[2], _goldColor);
            upgradesButtons[2].interactable = false; // Disable the upgrade button
            AudioManager.Instance.PlayAudioClip(1);
        }

        private void SetButtonColor(Button button, Color color)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = color;
            colorBlock.highlightedColor = color;
            colorBlock.pressedColor = color;
            colorBlock.selectedColor = color;
            button.colors = colorBlock;
        }
    }

}