using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class InGameMenuState : UIBaseState, IState
    {
        protected internal InGameMenuState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset) { }
        
        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            UIDoc.rootVisualElement.Q<Button>("Resume").clicked += UIManager.Instance.ResumeGame;
            UIDoc.rootVisualElement.Q<Button>("Options").clicked += UIManager.Instance.PressedOptionMenu;
            UIDoc.rootVisualElement.Q<Button>("Quit").clicked += Application.Quit;
            GameManager.Instance.Inputs.UI.Enable();
        }

        public void OnExit()
        {
            UIDoc.rootVisualElement.Q<Button>("Resume").clicked -= UIManager.Instance.ResumeGame;
            UIDoc.rootVisualElement.Q<Button>("Options").clicked -= UIManager.Instance.PressedOptionMenu;
            UIDoc.rootVisualElement.Q<Button>("Quit").clicked -= Application.Quit;
            GameManager.Instance.Inputs.UI.Disable();
        }
    }
}