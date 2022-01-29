using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidGroundState : IState
    {
        private readonly SquidController _squidController;
        public SquidGroundState(SquidController squidController)
        {
            _squidController = squidController;
        }

        public void OnEnter() => RotateUp();

        private void RotateUp() => _squidController.transform.rotation = Quaternion.Euler(0,0,0);
    }
}