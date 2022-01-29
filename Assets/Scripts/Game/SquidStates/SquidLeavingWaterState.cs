using MaxHelpers;
using UnityEngine;

namespace SquidStates
{
    public class SquidLeavingWaterState : IState
    {
        private readonly SquidController _squidController;
        private readonly SquidController.LeavingWaterParams _leavingWaterParams;
        public SquidLeavingWaterState(SquidController squidController, SquidController.LeavingWaterParams leavingWaterParams)
        {
            _squidController = squidController;
            _leavingWaterParams = leavingWaterParams;
        }

        public void OnEnter()
        {
            var up = _squidController.transform.up;
            var rightBoost = Scale( _leavingWaterParams.minimumAngleFromTop, 1f, _leavingWaterParams.minimumBoost, 1f, Vector2.Dot(up, new Vector2(Mathf.Cos(_leavingWaterParams.optimalAngle), Mathf.Sin(_leavingWaterParams.optimalAngle))));
            var leftBoost = Scale( _leavingWaterParams.minimumAngleFromTop, 1f, _leavingWaterParams.minimumBoost, 1f, Mathf.Abs(Vector2.Dot(up, new Vector2(Mathf.Cos(-_leavingWaterParams.optimalAngle), Mathf.Sin(-_leavingWaterParams.optimalAngle)))));
            Debug.Log($"{leftBoost} {rightBoost}");
            if (Vector2.Dot(_squidController.transform.up, Vector2.right) > 0)
                _squidController.Rb.velocity = _squidController.transform.up * _leavingWaterParams.force * rightBoost * _squidController.Rb.velocity.magnitude / _leavingWaterParams.maxVelocityForce;
            else
                _squidController.Rb.velocity = _squidController.transform.up * _leavingWaterParams.force * leftBoost * _squidController.Rb.velocity.magnitude / _leavingWaterParams.maxVelocityForce;
        }
        
        private static float Scale(float oldMin, float oldMax, float newMin, float newMax, float oldValue){
 
            var oldRange = (oldMax - oldMin);
            var newRange = (newMax - newMin);
            var newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
            return newValue;
        }
    }
}