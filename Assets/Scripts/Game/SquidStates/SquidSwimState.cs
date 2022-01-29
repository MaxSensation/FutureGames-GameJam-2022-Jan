using System;
using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidSwimState : IState
    {
        public Action OnEnteredState, OnExitState;
        private readonly SquidController _squidController;
        private readonly SquidController.InWaterParams _inWaterParams;
        private static readonly int IsSwimming = Animator.StringToHash("isSwimming");

        public SquidSwimState(SquidController squidController, SquidController.InWaterParams inInWaterParams)
        {
            _squidController = squidController;
            _inWaterParams = inInWaterParams;
        }

        public void OnEnter()
        {
            OnEnteredState?.Invoke();
            _squidController.Rb.gravityScale = 1f;
        }

        public void Tick()
        {
            _squidController.HandleMovement(_inWaterParams.speed, _inWaterParams.acceleration);
            _squidController.RotateTowards(_inWaterParams.rotationSpeed);
            _squidController.Animator.SetBool(IsSwimming, GameManager.Instance.Inputs.Player.Move.ReadValue<Vector2>().magnitude > 0f);
        }

        public void OnExit()
        {
            OnExitState?.Invoke();
            _squidController.Rb.gravityScale = 6f;
            _squidController.Animator.SetBool(IsSwimming, false);
        }
    }
}