using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidSwimState : IState
    {
        private readonly SquidController _squidController;
        private readonly SquidController.InWaterParams _inWaterParams;
        private static readonly int isSwimming = Animator.StringToHash("isSwimming");

        public SquidSwimState(SquidController squidController, SquidController.InWaterParams inInWaterParams)
        {
            _squidController = squidController;
            _inWaterParams = inInWaterParams;
        }

        public void OnEnter()
        {
            _squidController.Rb.gravityScale = 1f;
        }

        public void Tick()
        {
            _squidController.HandleMovement(_inWaterParams.speed, _inWaterParams.acceleration);
            _squidController.RotateTowards(_inWaterParams.rotationSpeed);
            _squidController.Animator.SetBool(isSwimming, GameManager.Instance.Inputs.Player.Move.ReadValue<Vector2>().magnitude > 0f);
        }

        public void OnExit()
        {
            _squidController.Rb.gravityScale = 6f;
            _squidController.Animator.SetBool(isSwimming, false);
        }
    }
}