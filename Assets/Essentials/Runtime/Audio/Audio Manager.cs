using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    [Service(ServiceLifetime.Global)]
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IAudioService
    {
        // —— Components ——
        private AudioSource _audioSource;

        // ==================
        // Life cycle
        // ==================
        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop        = false;
        }

        private void Update()
        {

        }

        // ==================
        // Core
        // ==================
        public AudioClip Play(string path)
        {
            AudioClip c = TryLoadAudioClip(path);
            if (c == null) return null;

            Pause();
            _audioSource.clip = c;
            _audioSource.Play();
            return c;
        }

        public void Play(AudioClip c)
        {
            if (c == null) return;

            Stop();
            _audioSource.clip = c;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        public void Pause()
        {
            _audioSource.Pause();
        }    

        // ==================
        // Interface
        // ==================
        public AudioClip TryLoadAudioClip(string path) => Resources.Load<AudioClip>(path);
        public bool IsPlaying() => _audioSource.isPlaying;
        public void SetAudioSource(AudioSource source) => _audioSource = source; 
    }
}