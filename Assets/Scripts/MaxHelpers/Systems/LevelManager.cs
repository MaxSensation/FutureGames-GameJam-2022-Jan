using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaxHelpers
{
    public class LevelManager : DataHandler<LevelManager, LevelData>
    {
        public Action OnStartLoadEvent, OnCompletedLoadEvent;
        public float LoadingProgress { get; private set; }
        
        private void Start() => InitData("level");

        public IEnumerator LoadScene(string sceneName)
        {
            OnStartLoadEvent?.Invoke();
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            do
            {
                yield return new WaitForSeconds(0.1f);
                LoadingProgress = 1f / scene.progress;
            } while (scene.progress < 0.9f);
            scene.allowSceneActivation = true;
            OnCompletedLoadEvent?.Invoke();
        }
        public void SetCurrentLevel(string level) => Data.currentLevel = level;
        public void LoadLastLevel()
        {
            StartCoroutine(LoadScene(Data.currentLevel));
        }
    }
}