using System;

namespace MaxHelpers
{
    public class GameManager : StaticInstance<GameManager>
    {
        public Action<float> OnWaterLevelChanged;
        public PlayerInputs Inputs { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            Inputs = new();
        }

        private void Start() => Inputs.Player.RestartLevel.performed += _ => LevelManager.Instance.LoadLastLevel();
    }
}