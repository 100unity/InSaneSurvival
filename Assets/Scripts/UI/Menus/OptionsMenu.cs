using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        [Header("Resolution")] [SerializeField]
        private TMP_Dropdown resolutionDropdown;

        [SerializeField] private Toggle fullscreenToggle;

        [Header("Quality")] [SerializeField] private TMP_Dropdown qualityDropdown;

        [Header("Volume")] [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider volumeSlider;


        private List<Resolution> _resolutions;

        private void Start()
        {
            AddResolutions();
            qualityDropdown.value = QualitySettings.GetQualityLevel();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            qualityDropdown.onValueChanged.AddListener(SetQuality);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        private void AddResolutions()
        {
            _resolutions = Screen.resolutions.ToList();
            List<string> resolutionNames = new List<string>();
            _resolutions.ForEach(res => resolutionNames.Add(res.ToString()));

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionNames);
            resolutionDropdown.value = _resolutions.IndexOf(Screen.currentResolution);
            resolutionDropdown.RefreshShownValue();
        }

        private void SetResolution(int resolutionIndex)
        {
            Resolution newResolution = _resolutions[resolutionIndex];
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        }

        private void SetFullscreen(bool fullscreen) => Screen.fullScreen = fullscreen;

        private void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

        private void SetVolume(float volume) => audioMixer.SetFloat("Volume", volume);
    }
}