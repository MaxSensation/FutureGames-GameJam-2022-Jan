using System;
using UnityEngine;

namespace MaxHelpers
{
    [Serializable]
    public struct SettingsData : ISave
    {
        [Header("Volume")]
        [Range(0.001f,1f)] public float masterVolume;
        [Range(0.001f,1f)] public float musicVolume;
        [Range(0.001f,1f)] public float soundFXVolume;
        [Header("Quality")]
        public bool fullScreen;
        [Tooltip("This needs to be a string because of how the saving system works, different settings: Low, Medium, High")] public string qualitySettings;
        public string resolution;
        [Header("Controls")]
        [Range(0.001f,1f)]public float sensitivity;
    }
}