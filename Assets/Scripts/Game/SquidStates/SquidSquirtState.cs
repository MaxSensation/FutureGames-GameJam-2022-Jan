using System;
using MaxHelpers;

namespace SquidStates
{
    public class SquidSquirtState : IState
    {
        public Action OnEnteredState;
        private readonly SquidController _squidController;
        private readonly float _force;

        public SquidSquirtState(SquidController squidController, float force)
        {
            _squidController = squidController;
            _force = force;
        }

        public void OnEnter()
        {
            OnEnteredState?.Invoke();
            _squidController.Squirt();
        }
    }
}