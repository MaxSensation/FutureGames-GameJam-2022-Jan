using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class OptionsState : UIBaseState, IState
    {
        private Slider _masterSlider, _musicSlider, _soundFxSlider, _sensitivitySlider;
        private DropdownField _qualityDropdown, _resolutionDropDown;
        private Toggle _fullscreenToggle;

        protected internal OptionsState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset) { }
        
        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            GetAllUIElements();
            InitDataToUI();
            RegisterEvents();
            GameManager.Instance.Inputs.UI.Enable();
        }
        public void OnExit()
        {
            UnregisterEvents();
            GameManager.Instance.Inputs.UI.Disable();
            SettingsManager.Instance.SaveData();
        }

        private void RegisterEvents()
        {
            UIDoc.rootVisualElement.Q<Button>("GoBack").clicked += UIManager.Instance.GoToPreviousState;
            _masterSlider.RegisterValueChangedCallback(SettingsManager.Instance.UpdateMasterVolume);
            _musicSlider.RegisterValueChangedCallback(SettingsManager.Instance.UpdateMusicVolume);
            _soundFxSlider.RegisterValueChangedCallback(SettingsManager.Instance.UpdateSoundFxVolume);
            _qualityDropdown.RegisterValueChangedCallback(SettingsManager.Instance.UpdateGraphicSetting);
            _resolutionDropDown.RegisterValueChangedCallback(SettingsManager.Instance.UpdateResolution);
            _fullscreenToggle.RegisterValueChangedCallback(SettingsManager.Instance.UpdateFullscreen);
            _sensitivitySlider.RegisterValueChangedCallback(SettingsManager.Instance.UpdateSensitivity);
        }

        private void UnregisterEvents()
        {
            UIDoc.rootVisualElement.Q<Button>("GoBack").clicked -= UIManager.Instance.GoToPreviousState;
            _masterSlider.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateMasterVolume);
            _musicSlider.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateMusicVolume);
            _soundFxSlider.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateSoundFxVolume);
            _sensitivitySlider.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateSensitivity);
            _qualityDropdown.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateGraphicSetting);
            _fullscreenToggle.UnregisterValueChangedCallback(SettingsManager.Instance.UpdateFullscreen);
        }

        private void GetAllUIElements()
        {
            _masterSlider = UIDoc.rootVisualElement.Q<Slider>("MasterSlider");
            _musicSlider = UIDoc.rootVisualElement.Q<Slider>("MusicSlider");
            _soundFxSlider = UIDoc.rootVisualElement.Q<Slider>("SoundFXSlider");
            _qualityDropdown = UIDoc.rootVisualElement.Q<DropdownField>("GraphicSetting");
            _resolutionDropDown = UIDoc.rootVisualElement.Q<DropdownField>("ResolutionSetting");
            _fullscreenToggle = UIDoc.rootVisualElement.Q<Toggle>("FullscreenToggle");
            _sensitivitySlider = UIDoc.rootVisualElement.Q<Slider>("SensitivitySlider");
        }
        
        private void InitDataToUI()
        {
            _masterSlider.SetValueWithoutNotify(SettingsManager.Instance.VolumeMaster);
            _musicSlider.SetValueWithoutNotify(SettingsManager.Instance.VolumeMusic);
            _soundFxSlider.SetValueWithoutNotify(SettingsManager.Instance.VolumeSoundFX);
            _fullscreenToggle.SetValueWithoutNotify(SettingsManager.Instance.Fullscreen);
            _qualityDropdown.SetValueWithoutNotify(SettingsManager.Instance.QualitySettings);
            _resolutionDropDown.choices = Screen.resolutions.Select(i => i.width + "x" + i.height).ToList();
            _resolutionDropDown.SetValueWithoutNotify(Screen.currentResolution.width + "x" + Screen.currentResolution.height);
            _sensitivitySlider.SetValueWithoutNotify(SettingsManager.Instance.Sensitivity);
        }
    }
}