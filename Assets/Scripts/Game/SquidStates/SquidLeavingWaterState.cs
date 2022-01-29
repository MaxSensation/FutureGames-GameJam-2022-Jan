using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidLeavingWaterState : IState
    {
        private readonly SquidController _squidController;
        private readonly float _force;

        public SquidLeavingWaterState(SquidController squidController, float force)
        {
            _squidController = squidController;
            _force = force;
        }

        public void OnEnter()
        {
            Debug.Log("Enetered LeavingWater");
            _squidController.Rb.velocity = _squidController.transform.up * _force;
        }
    }
}