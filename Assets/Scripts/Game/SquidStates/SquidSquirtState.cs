using MaxHelpers;

namespace SquidStates
{
    public class SquidSquirtState : IState
    {
        private readonly SquidController _squidController;
        private readonly float _force;

        public SquidSquirtState(SquidController squidController, float jumpHeight)
        {
            _squidController = squidController;
            _force = jumpHeight;
        }

        public void OnEnter() => _squidController.Rb.velocity = _squidController.transform.up * _force;
    }
}