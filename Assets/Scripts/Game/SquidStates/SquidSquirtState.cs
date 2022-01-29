using MaxHelpers;

namespace SquidStates
{
    public class SquidSquirtState : IState
    {
        private readonly SquidController _squidController;
        private readonly float _force;

        public SquidSquirtState(SquidController squidController, float force)
        {
            _squidController = squidController;
            _force = force;
        }

        public void OnEnter() => _squidController.Rb.velocity = _squidController.transform.up * _force;
    }
}