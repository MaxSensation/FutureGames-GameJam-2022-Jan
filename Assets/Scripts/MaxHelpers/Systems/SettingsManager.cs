using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class SettingsManager : DataHandler<SettingsManager, SettingsData>
    {
        public float VolumeMaster => Data.masterVolume;
        public float VolumeMusic => Data.musicVolume;
        public float VolumeSoundFX => Data.soundFXVolume;
        public bool Fullscreen => Data.fullScreen;
        public string QualitySettings => Data.qualitySettings;
        public float Sensitivity => Data.sensitivity;
        
        private void Start()
        {
            InitData("settings");
            InitSettings();
        }
        
        private void InitSettings()
        {
            AudioManager.Instance.SetVolume("Master", VolumeMaster);
            AudioManager.Instance.SetVolume("Music", VolumeMusic);
            AudioManager.Instance.SetVolume("SoundFX", VolumeSoundFX);
            SetQuality(QualitySettings);
            SetFullscreen(Fullscreen);
        }

        private static void SetQuality(string quality)
        {
            switch (quality)
            {
                case "Low":
                    UnityEngine.QualitySettings.SetQualityLevel (0, true);
                    break;
                case "Normal":
                    UnityEngine.QualitySettings.SetQualityLevel (3, true);
                    break;
                case "High":
                    UnityEngine.QualitySettings.SetQualityLevel (5, true);
                    break;
            }
        }

        private static void SetResolution(string resolution, bool fullScreen)
        {
            var combinedString = resolution.Split("x");
            int.TryParse(combinedString[0], out var width);
            int.TryParse(combinedString[1], out var height);
            Screen.SetResolution(width, height, fullScreen);
        }
        private static void SetFullscreen(bool fullscreen) => Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        public void UpdateFullscreen(ChangeEvent<bool> toggleEvent)
        {
            Data.fullScreen = toggleEvent.newValue;
            SetFullscreen(toggleEvent.newValue);
        }

        public void UpdateGraphicSetting(ChangeEvent<string> dropdownEvent)
        {
            Data.qualitySettings = dropdownEvent.newValue;
            SetQuality(dropdownEvent.newValue);
        }

        public void UpdateResolution(ChangeEvent<string> dropdownEvent)
        {
            Data.resolution = dropdownEvent.newValue;
            SetResolution(Data.resolution, Data.fullScreen);
        }
        
        public void UpdateSensitivity(ChangeEvent<float> sliderEvent)
        {
            Data.sensitivity = sliderEvent.newValue;
        }
        
        public void UpdateMasterVolume(ChangeEvent<float> sliderEvent)
        {
            Data.masterVolume = sliderEvent.newValue;
            AudioManager.Instance.SetVolume("Master", sliderEvent.newValue);
        }
        
        public void UpdateMusicVolume(ChangeEvent<float> sliderEvent)
        {
            Data.musicVolume = sliderEvent.newValue;
            AudioManager.Instance.SetVolume("Music", sliderEvent.newValue);
        }

        public void UpdateSoundFxVolume(ChangeEvent<float> sliderEvent)
        {
            Data.soundFXVolume = sliderEvent.newValue;
            AudioManager.Instance.SetVolume("SoundFX", sliderEvent.newValue);
        }
    }
}