using MaxHelpers;

namespace SquidStates
{
    public class SquidSquirtState : IState
    {
        private readonly SquidController _squidController;
        private readonly float _jumpHeight;

        public SquidSquirtState(SquidController squidController, float jumpHeight)
        {
            _squidController = squidController;
            _jumpHeight = jumpHeight;
        }

        public void OnEnter() => Squirt();

        private void Squirt() => _squidController.Rb.velocity = _squidController.transform.up * _jumpHeight ;
    }
}