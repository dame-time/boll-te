using Player;
using UnityEngine;

namespace Audio
{
    public class MicInput : MonoBehaviour
    {
        [SerializeField] private int sampleRate = 44100;
        [SerializeField] private int fftSize = 256;
        [SerializeField] private float blowFrequencyMin = 0f;
        [SerializeField] private float blowFrequencyMax = 2500f;
        [SerializeField] private float blowMin = 0.0f;
        [SerializeField] private float blowMax = 10.0f;
    
        private float[] _samples;
        private float[] _spectrum;
        
        public int progress = 0;
        
        private string _micDevice;
        private AudioSource _audioSource;

        private Station _station;

        private PlayerMovement playerRef;

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

                while (Microphone.GetPosition(_micDevice) <= 0) { }
                _audioSource.Play();

                _samples = new float[fftSize];
                _spectrum = new float[fftSize];
            }
            else
                Debug.LogError("No microphone detected!");
        }

        private void Update()
        {
            if (!_audioSource || !_audioSource.clip) return;

            if (progress >= 100)
            {
                _station.initialStationStatus = StationBothType.Grabbable;
                
                progress = 0;
                playerRef.bubbleProgression.gameObject.SetActive(false);
                playerRef.bubbleProgression.value = 0;
                return;
            }
        
            _audioSource.clip.GetData(_samples, 0);
            AudioListener.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);

            if (IsBlowDetected(_spectrum) && _station.stationType == StationType.Both && _station.initialStationStatus == StationBothType.Progress)
            {
                Debug.Log("Blow detected!");
                progress++;
                print("progress blow is : " + progress);
                if (playerRef)
                {
                    print("progress blow is : " + progress);
                    playerRef.bubbleProgression.value = progress;
                }
            }
        }

        private bool IsBlowDetected(float[] spectrum)
        {
            var sum = 0f;

            var minIndex = Mathf.FloorToInt(blowFrequencyMin / (sampleRate / 2f) * spectrum.Length);
            var maxIndex = Mathf.CeilToInt(blowFrequencyMax / (sampleRate / 2f) * spectrum.Length);

            for (var i = minIndex; i <= maxIndex; i++) sum += spectrum[i];
            //Debug.Log("sum inside blow detected is: " + sum);
            return sum > blowMin && sum < blowMax;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.tag.Equals("Player")) return;
            playerRef = other.GetComponent<PlayerMovement>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.tag.Equals("Player")) return;            
            playerRef = null;
        }
    }
}