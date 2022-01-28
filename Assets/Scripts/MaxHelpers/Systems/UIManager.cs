using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers {
    public class UIManager : StaticInstance<UIManager>
    {
        [SerializeField] private VisualTreeAsset 
            mainMenuUI, 
            inGameMenuUI,
            inGameUI,
            optionsUI,
            loadingUI;

        private readonly StateMachine _stateMachine = new();
        private OptionsState _optionsState;
        private InGameState _inGameState;
        protected void Start()
        {
            var uiDocument = GetComponent<UIDocument>();
            // Create states
            MainMenuState mainMenuState = new(uiDocument, mainMenuUI);
            InGameMenuState inGameMenuState = new(uiDocument, inGameMenuUI);
            _inGameState = new InGameState(uiDocument, inGameUI);
            _optionsState = new OptionsState(uiDocument, optionsUI);
            LoadingState loadingState = new(uiDocument, loadingUI);
            // Listen to events
            LevelManager.Instance.OnStartLoadEvent += () => _stateMachine.SetState(loadingState);
            LevelManager.Instance.OnCompletedLoadEvent += () => _stateMachine.SetState(_inGameState);
            // Add Transitions
            _stateMachine.AddTransition(_inGameState, inGameMenuState, GameManager.Instance.Inputs.Player.OpenGameMenu.IsPressed);
            _stateMachine.AddTransition(inGameMenuState, _inGameState, GameManager.Instance.Inputs.UI.ExitMenu.IsPressed);
            _stateMachine.AddTransition(_optionsState, inGameMenuState, () => IsExitPressedAndPrevState(inGameMenuState));
            _stateMachine.AddTransition(_optionsState, mainMenuState, () => IsExitPressedAndPrevState(mainMenuState));
            // Set start state
            _stateMachine.SetState(mainMenuState);
        }
        private void Update() => _stateMachine.Tick();
        private bool IsExitPressedAndPrevState(IState state) => GameManager.Instance.Inputs.UI.ExitMenu.IsPressed() && _stateMachine.GetPreviousState() == state;
        public void GoToPreviousState() => _stateMachine.GoToPreviousState();
        public void PressedOptionMenu() => _stateMachine.SetState(_optionsState);
        public void ResumeGame() => _stateMachine.SetState(_inGameState);
    }
}