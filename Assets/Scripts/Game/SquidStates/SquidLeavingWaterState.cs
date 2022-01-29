using System;
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
            var transformUp = _squidController.transform.up;
            var combindDir = (_squidController.Rb.velocity.normalized + (Vector2)transformUp).normalized;
            var rightDot = Vector2.Dot(combindDir, new Vector2(Mathf.Cos(_leavingWaterParams.optimalAngle), Mathf.Sin(_leavingWaterParams.optimalAngle)));
            var leftDot = Mathf.Abs(Vector2.Dot(combindDir, new Vector2(Mathf.Cos(-_leavingWaterParams.optimalAngle), Mathf.Sin(-_leavingWaterParams.optimalAngle))));
            //Debug.Log($"LeftDot: {rightDot}, RightDot: {leftDot}");
            var rightBoost = Scale( _leavingWaterParams.minimumAngleFromTop, 1f, _leavingWaterParams.minimumBoost, _leavingWaterParams.force, rightDot);
            var leftBoost = Scale( _leavingWaterParams.minimumAngleFromTop, 1f, _leavingWaterParams.minimumBoost, _leavingWaterParams.force, leftDot);
            //Debug.Log($"RightBoost: {rightBoost}, LeftBoost: {leftBoost}");
            var newVelocity = Vector2.zero;
            if (_squidController.Rb.velocity.x > 0f) 
                newVelocity = (transformUp + new Vector3(_leavingWaterParams.horizontalBoost, 0, 0)).normalized * (rightBoost * _squidController.Rb.velocity.magnitude / _leavingWaterParams.maxVelocityForce);
            else if(_squidController.Rb.velocity.x < 0f)
                newVelocity = (transformUp + new Vector3(-_leavingWaterParams.horizontalBoost, 0, 0)).normalized * (leftBoost * _squidController.Rb.velocity.magnitude / _leavingWaterParams.maxVelocityForce);
            else
                newVelocity = transformUp * _leavingWaterParams.fullyVerticalBoost * (_squidController.Rb.velocity.magnitude / _leavingWaterParams.maxVelocityForce);
            _squidController.Rb.velocity = newVelocity;
        }
        
        private static float Scale(float oldMin, float oldMax, float newMin, float newMax, float oldValue){
 
            var oldRange = (oldMax - oldMin);
            var newRange = (newMax - newMin);
            var newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
            return newValue;
        }
    }
}