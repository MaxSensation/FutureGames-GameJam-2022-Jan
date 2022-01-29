using MaxHelpers;

namespace SquidStates
{
    public class SquidDeathState : IState
    {
        private SquidController _squidController;
        public SquidDeathState(SquidController squidController)
        {
            _squidController = squidController;
        }

        public void OnEnter()
        {
            _squidController.gameObject.SetActive(false);
        }
    }
}