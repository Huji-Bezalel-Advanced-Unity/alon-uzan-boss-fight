using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    /// <summary>
    /// Manages and controls all audio playback in the game.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField]
        private AudioSource backgroundSource; // For background music
        
        [SerializeField]
        private AudioSource itemSource; // For itemized sounds
        
        [SerializeField]
        private AudioClip[] audioClips; // Collection of audio clips

        /// <summary>
        /// Singleton instance of AudioManager.
        /// </summary>
        public static AudioManager Instance { get; private set; }

        // End Of Local Variables

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("AudioManager already exists");
                Destroy(gameObject);
            }
        }

        public void PlayAudioClip(int index)
        {
            if (index >= 2)
            {
                backgroundSource.clip = audioClips[index];
                backgroundSource.Play();
            }
            else
            {
                itemSource.PlayOneShot(audioClips[index]);
            }
        }
    }
}