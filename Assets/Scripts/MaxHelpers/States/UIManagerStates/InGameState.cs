using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class InGameState : UIBaseState, IState
    {
        private ProgressBar _progressBar;
        private VisualElement _inkOne, _inkTwo, _inkThree, _winScreen;

        protected internal InGameState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset)
        { }
        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            UIDoc.rootVisualElement.Q<VisualElement>("MainPanel").style.display = DisplayStyle.Flex;
            GameManager.Instance.Inputs.Player.Enable();
            Time.timeScale = 1f;
            _progressBar = UIDoc.rootVisualElement.Q<ProgressBar>("WaterLevel");
            _inkOne = UIDoc.rootVisualElement.Q<VisualElement>("InkOne");
            _inkTwo = UIDoc.rootVisualElement.Q<VisualElement>("InkTwo");
            _inkThree = UIDoc.rootVisualElement.Q<VisualElement>("InkThree");
            _winScreen = UIDoc.rootVisualElement.Q<VisualElement>("WinScreen");
            GameManager.Instance.OnWinEvent += Win;
        }

        private void Win() => _winScreen.style.visibility = Visibility.Visible;

        public void Tick()
        {
            _progressBar.value = UIManager.Instance.CurrentWaterLevel;
            UpdateInkAmount();
        }

        private void UpdateInkAmount()
        {
            switch (UIManager.Instance.CurrentInks)
            {
                case 0:
                    _inkOne.style.visibility = Visibility.Hidden;
                    _inkTwo.style.visibility = Visibility.Hidden;
                    _inkThree.style.visibility = Visibility.Hidden;
                    break;
                case 1:
                    _inkOne.style.visibility = Visibility.Visible;
                    _inkTwo.style.visibility = Visibility.Hidden;
                    _inkThree.style.visibility = Visibility.Hidden;
                    break;
                case 2:
                    _inkOne.style.visibility = Visibility.Visible;
                    _inkTwo.style.visibility = Visibility.Visible;
                    _inkThree.style.visibility = Visibility.Hidden;
                    break;
                case 3:
                    _inkOne.style.visibility = Visibility.Visible;
                    _inkTwo.style.visibility = Visibility.Visible;
                    _inkThree.style.visibility = Visibility.Visible;
                    break;
            }
        }

        public void OnExit()
        {
            GameManager.Instance.Inputs.Player.Disable();
            Time.timeScale = 0f;
        }
    }
}