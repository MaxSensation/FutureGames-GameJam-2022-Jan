using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidAirState : IState
    {
        private readonly SquidController _squidController;
        private readonly SquidController.InAirParams _inAirParams;
        private readonly float _speed, _acceleration;
        private readonly float _rotationSpeed;
        private readonly float _airControl;
        private Quaternion _lastRot;

        public SquidAirState(SquidController squidController, SquidController.InAirParams inAirParams)
        {
            _squidController = squidController;
            _inAirParams = inAirParams;
        }

        public void Tick()
        {
            _squidController.HandleMovement(_inAirParams.speed, _inAirParams.acceleration, _inAirParams.deacceleration, _inAirParams.control, false);
            _lastRot = _squidController.RotateTowards(_inAirParams.rotationSpeed, _lastRot, _inAirParams.rotateVelocity);
        }
    }
}