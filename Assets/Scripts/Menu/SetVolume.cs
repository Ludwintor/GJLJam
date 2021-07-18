using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GJLJam
{
    public class SetVolume : MonoBehaviour
    {
        [SerializeField]
        private Slider masterSlider;
        [SerializeField]
        private Slider musicSlider;
        [SerializeField]
        private Slider soundSlider;
        [SerializeField]
        private AudioMixer mixer;
        [SerializeField]
        private AudioSource testSource;

        private bool played;

        private void Start()
        {
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            soundSlider.onValueChanged.AddListener(SetSoundVolume);

            masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
            soundSlider.value = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        }

        public void SetMasterVolume(float sliderValue)
        {
            mixer.SetFloat("masterVolume", CalculateValue(sliderValue));
            PlayerPrefs.SetFloat("masterVolume", sliderValue);
        }

        public void SetMusicVolume(float sliderValue)
        {
            mixer.SetFloat("musicVolume", CalculateValue(sliderValue));
            PlayerPrefs.SetFloat("musicVolume", sliderValue);
        }

        public void SetSoundVolume(float sliderValue)
        {
            mixer.SetFloat("soundVolume", CalculateValue(sliderValue));
            PlayerPrefs.SetFloat("soundVolume", sliderValue);

            if (!played)
            {
                played = true;
                return;
            }
            TestSound();
        }

        public void TestSound()
        {
            if (!testSource.isPlaying)
                testSource.Play();
        }

        private float CalculateValue(float value)
        {
            return Mathf.Log10(value) * 20;
        }
    }
}