using System.Collections.Generic;
using System.Linq;
using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        [Header("Resolution")] [Tooltip("The dropdown for choosing a resolution")] [SerializeField]
        private TMP_Dropdown resolutionDropdown;

        [Tooltip("If the game should run in fullscreen")] [SerializeField]
        private Toggle fullscreenToggle;

        [Header("Quality")] [Tooltip("The dropdown for choosing a quality-level")] [SerializeField]
        private TMP_Dropdown qualityDropdown;

        [Header("Volume")] [Tooltip("The audio mixer of the game, should be the main-mixer")] [SerializeField]
        private AudioMixer audioMixer;

        [Tooltip("The volume slider, used for changing the volume")] [SerializeField]
        private Slider volumeSlider;

        [Header("Mouse Sensitivity")] [Tooltip("The mouse sensitivity slider")] [SerializeField]
        private Slider mouseSensitivitySlider;

        public static float MouseSensitivity { get; private set; } = 1;

        private List<Resolution> _resolutions;

        /// <summary>
        /// Sets all listeners
        /// </summary>
        private void Awake()
        {
            AddResolutions();
            qualityDropdown.value = QualitySettings.GetQualityLevel();

            fullscreenToggle.isOn = Screen.fullScreen;
            audioMixer.GetFloat(Consts.Utils.AUDIO_MANAGER_VOLUME, out float volume);
            volumeSlider.value = volume;
            mouseSensitivitySlider.value = MouseSensitivity;

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            qualityDropdown.onValueChanged.AddListener(SetQuality);
            volumeSlider.onValueChanged.AddListener(SetVolume);
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }

        /// <summary>
        /// Fills the dropdown with all supported resolutions and chooses the currently used one
        /// </summary>
        private void AddResolutions()
        {
            _resolutions = Screen.resolutions.ToList();
            List<string> resolutionNames = new List<string>();
            _resolutions.ForEach(res => resolutionNames.Add(res.ToString()));

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionNames);
            resolutionDropdown.value = _resolutions.FindIndex(res =>
                res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height);
            resolutionDropdown.RefreshShownValue();
        }

        /// <summary>
        /// Sets a new resolution
        /// </summary>
        /// <param name="resolutionIndex">The index of the resolution (from the dropdown)</param>
        private void SetResolution(int resolutionIndex)
        {
            Resolution newResolution = _resolutions[resolutionIndex];
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        }

        /// <summary>
        /// Enables/Disables Fullscreen
        /// </summary>
        /// <param name="fullscreen">Whether it is fullscreen or not</param>
        private void SetFullscreen(bool fullscreen) => Screen.fullScreen = fullscreen;

        /// <summary>
        /// Sets a new quality-level
        /// </summary>
        /// <param name="qualityIndex">The index of the quality-level (from the dropdown)</param>
        private void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

        /// <summary>
        /// Sets a new volume level in the audio-mixer
        /// </summary>
        /// <param name="volume"></param>
        private void SetVolume(float volume) => audioMixer.SetFloat(Consts.Utils.AUDIO_MANAGER_VOLUME, volume);

        /// <summary>
        /// Sets the mouse sensitivity for rotating the camera
        /// </summary>
        /// <param name="sensitivity"></param>
        private void SetMouseSensitivity(float sensitivity) => MouseSensitivity = sensitivity;
    }
}