using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class LoadingState : UIBaseState, IState
    {
        private ProgressBar _progressBar;
        private const float TransitionSpeed = 3;
        private float _targetPercentage;

        protected internal LoadingState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset) { }
        
        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            _progressBar = UIDoc.rootVisualElement.Q<ProgressBar>();
            _progressBar.value = 0f;
            _targetPercentage = 0f;
        }

        public void Tick()
        {
            _targetPercentage = LevelManager.Instance.LoadingProgress;
            _progressBar.value = Mathf.MoveTowards(_progressBar.value, _targetPercentage, TransitionSpeed * Time.deltaTime);
        }
    }
}