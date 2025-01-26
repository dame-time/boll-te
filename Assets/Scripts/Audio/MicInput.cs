using System.Linq;
using Player;
using UnityEngine;

namespace Audio
{
    public class MicInput : MonoBehaviour
    {
        [SerializeField] private int sampleRate = 44100;
        [SerializeField] private int fftSize = 1024; // Size of the sample buffer
        [SerializeField] private float blowThreshold = 0.008f; // Minimum amplitude for detecting a blow
        [SerializeField] private float blowMinDuration = 0.005f; // Minimum duration for sustained amplitude to be considered a blow

        private float[] _samples;

        private string _micDevice;
        private AudioSource _audioSource;

        private float _blowTime;
        private Station _station;
        private PlayerMovement _playerRef;

        public int progress = 0;

        private void Start()
        {
            _station = GetComponent<Station>();

            if (Microphone.devices.Length > 0)
            {
                _micDevice = Microphone.devices[0];
                Debug.Log($"Using Microphone: {_micDevice}");

                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = Microphone.Start(_micDevice, true, 1, sampleRate);
                _audioSource.loop = true;
                _audioSource.mute = true;

                while (Microphone.GetPosition(_micDevice) <= 0) { }
                _audioSource.Play();

                _samples = new float[fftSize];
            }
            else
            {
                Debug.LogError("No microphone detected!");
            }
        }

        private void Update()
        {
            if (!_audioSource || !_audioSource.clip) return;

            if (progress >= 10)
            {
                _station.initialStationStatus = StationBothType.Grabbable;

                progress = 0;
                _playerRef.bubbleProgression.gameObject.SetActive(false);
                _playerRef.bubbleProgression.value = 0;
                return;
            }

            _audioSource.clip.GetData(_samples, 0);

            if (!IsBlowDetected() || _station.stationType != StationType.Both ||
                _station.initialStationStatus != StationBothType.Progress) return;
            progress++;

            if (_playerRef) _playerRef.bubbleProgression.value = progress * 10;
        }

        private bool IsBlowDetected()
        {
            var sum = _samples.Sum(sample => Mathf.Abs(sample));

            var averageAmplitude = sum / _samples.Length;

            if (!(averageAmplitude > blowThreshold)) return false;
            _blowTime += Time.deltaTime;
            if (!(_blowTime >= blowMinDuration)) return false;
            
            _blowTime = 0f;
            return true;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _playerRef = other.GetComponent<PlayerMovement>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _playerRef = null;
        }
    }
}
