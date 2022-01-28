using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class InGameState : UIBaseState, IState
    {
        protected internal InGameState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset) { }
        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            UIDoc.rootVisualElement.Q<VisualElement>("MainPanel").style.display = DisplayStyle.Flex;
            GameManager.Instance.Inputs.Player.Enable();
            Time.timeScale = 1f;
        }
        public void OnExit()
        {
            GameManager.Instance.Inputs.Player.Disable();
            Time.timeScale = 0f;
        }
    }
}