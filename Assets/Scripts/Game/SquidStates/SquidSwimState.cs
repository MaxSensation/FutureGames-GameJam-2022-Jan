using System;
using MaxHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquidStates
{
    public class SquidSwimState : IState
    {
        public Action OnEnteredState, OnExitState;
        private readonly SquidController _squidController;
        private readonly SquidController.InWaterParams _inWaterParams;
        private static readonly int IsSwimming = Animator.StringToHash("isSwimming");
        private AudioClip[] _audioClips;
        private float _lastSwimSoundTime;
        private const float TimeBetweenSound = 0.5f;

        public SquidSwimState(SquidController squidController, SquidController.InWaterParams inInWaterParams, AudioClip[] sounds)
        {
            _squidController = squidController;
            _inWaterParams = inInWaterParams;
            _audioClips = sounds;
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
            if (GameManager.Instance.Inputs.Player.Move.ReadValue<Vector2>().magnitude > 0f)
            {
                
                _squidController.Animator.SetBool(IsSwimming, true);
                if (!(Time.time - _lastSwimSoundTime > TimeBetweenSound)) return;
                AudioManager.Instance.PlaySound(_audioClips[Random.Range(0,_audioClips.Length)], 0.5f);
                _lastSwimSoundTime = Time.time;
            }
            else
            {
                _squidController.Animator.SetBool(IsSwimming, false);
            }
        }

        public void OnExit()
        {
            OnExitState?.Invoke();
            _squidController.Rb.gravityScale = 6f;
            _squidController.Animator.SetBool(IsSwimming, false);
        }
    }
}