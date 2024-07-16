using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource backgroundSource; // For background music
        [SerializeField] private AudioSource itemSource; // For itemized sounds
        [SerializeField] private AudioClip[] audioClips;

        public static AudioManager Instance { get; private set; }

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
                Destroy(gameObject); // Ensure only one instance exists
            }
        }

        public void PlayAudioClip(int index)
        {
            if (index >= 2)
            {
                // Handles background audio (0 and 1 are for background)
                backgroundSource.clip = audioClips[index];
                backgroundSource.Play();
            }
            else
            {
                // Handles itemized sounds (2 and 3 are itemized sounds)
                itemSource.PlayOneShot(audioClips[index]);
            }
        }
        
    }
}