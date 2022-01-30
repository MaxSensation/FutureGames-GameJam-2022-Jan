using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidDeathState : IState
    {
        private SquidController _squidController;
        private AudioClip _deathSound;
        public SquidDeathState(SquidController squidController, AudioClip deathSound)
        {
            _squidController = squidController;
            _deathSound = deathSound;
        }

        public void OnEnter()
        {
            AudioManager.Instance.PlaySound(_deathSound);
            SquidController.OnDiedEvent?.Invoke();
        }
    }
}