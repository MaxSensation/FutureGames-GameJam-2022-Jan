using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class MainMenuState : UIBaseState, IState
    {
        protected internal MainMenuState(UIDocument uiDoc, VisualTreeAsset asset) : base(uiDoc, asset) { }

        public void OnEnter()
        {
            UIDoc.visualTreeAsset = Asset;
            UIDoc.rootVisualElement.Q<Button>("StartGame").clicked += LevelManager.Instance.LoadLastLevel;
            UIDoc.rootVisualElement.Q<Button>("Options").clicked += UIManager.Instance.PressedOptionMenu;
            UIDoc.rootVisualElement.Q<Button>("Quit").clicked += Application.Quit;
            GameManager.Instance.Inputs.UI.Enable();
        }
        
        public void OnExit()
        {
            UIDoc.rootVisualElement.Q<Button>("StartGame").clicked -= LevelManager.Instance.LoadLastLevel;
            UIDoc.rootVisualElement.Q<Button>("Options").clicked -= UIManager.Instance.PressedOptionMenu;
            UIDoc.rootVisualElement.Q<Button>("Quit").clicked -= Application.Quit;
            GameManager.Instance.Inputs.UI.Disable();
        }
    }
}