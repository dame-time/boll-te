using Player;
using UnityEngine;

namespace Audio
{
    public class MicInput : MonoBehaviour
    {
        [SerializeField] private int sampleRate = 44100;
        [SerializeField] private int fftSize = 2048;
        [SerializeField] private float blowFrequencyMin = 50f;
        [SerializeField] private float blowFrequencyMax = 500f;
        [SerializeField] private float blowMin = 0.0f;
        [SerializeField] private float blowMax = 1.0f;
    
        private float[] _samples;
        private float[] _spectrum;
        
        public int progress = 0;
        
        private string _micDevice;
        private AudioSource _audioSource;

        private Station _station;

        private void Start()
        {
            _station = GetComponent<Station>();
            
            if (Microphone.devices.Length > 0)
            {
                _micDevice = Microphone.devices[0];
                Debug.Log($"Using Microphone: {_micDevice}");

                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = Microphone.Start(_micDevice, true, 10, sampleRate);
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
                return;
            }
        
            _audioSource.clip.GetData(_samples, 0);
            AudioListener.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);

            if (IsBlowDetected(_spectrum) && _station.stationType == StationType.Both && _station.initialStationStatus == StationBothType.Progress)
            {
                Debug.Log("Blow detected!");
                progress++;
            }
        }

        private bool IsBlowDetected(float[] spectrum)
        {
            var sum = 0f;

            var minIndex = Mathf.FloorToInt(blowFrequencyMin / (sampleRate / 2f) * spectrum.Length);
            var maxIndex = Mathf.CeilToInt(blowFrequencyMax / (sampleRate / 2f) * spectrum.Length);

            for (var i = minIndex; i <= maxIndex; i++) sum += spectrum[i];

            return sum > blowMin && sum < blowMax;
        }
    }
}