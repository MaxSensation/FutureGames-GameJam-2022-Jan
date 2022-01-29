using MaxHelpers;

namespace SquidStates
{
    public class SquidUnderwaterState : IState
    {
        private readonly SquidController _squidController;
        private readonly SquidController.InWaterParams _inWaterParams;
        
        public SquidUnderwaterState(SquidController squidController, SquidController.InWaterParams inInWaterParams)
        {
            _squidController = squidController;
            _inWaterParams = inInWaterParams;
        }

        public void OnEnter() => _squidController.Rb.gravityScale = 1f;

        public void Tick()
        {
            _squidController.HandleMovement(_inWaterParams.speed, _inWaterParams.acceleration);
            _squidController.RotateTowards(_inWaterParams.rotationSpeed);
        }

        public void OnExit() => _squidController.Rb.gravityScale = 4f;
    }
}