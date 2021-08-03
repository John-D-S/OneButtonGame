using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleController : MonoBehaviour
{
    
    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float motorForceSteeringDifference = 100f;
    [SerializeField] private float motorVelocity = 1000f;
    [SerializeField] private float motorVelocitySteeringDifference = 500f;
    [SerializeField] private List<HingeJoint> leftWheels = new List<HingeJoint>();
    [SerializeField] private List<HingeJoint> rightWheels = new List<HingeJoint>();
    
    [SerializeField] private float steeringArmAngle = 30;
    [SerializeField] private float steeringLerpSpeed = 0.04f;
    [SerializeField] private List<HingeJoint> leftArms = new List<HingeJoint>();
    [SerializeField] private List<HingeJoint> rightArms = new List<HingeJoint>();

    private void FixedUpdate()
    {
        float leftWheelsVelocity = motorVelocity + motorVelocitySteeringDifference * Mathf.Clamp01(Input.GetAxisRaw("Horizontal"));
        float leftWheelsForce = motorForce + motorForceSteeringDifference * Mathf.Clamp01(Input.GetAxisRaw("Horizontal"));
        float rightWheelsVelocity = motorVelocity + motorVelocitySteeringDifference * Mathf.Clamp01(-Input.GetAxisRaw("Horizontal"));
        float rightWheelsForce = motorForce + motorForceSteeringDifference * Mathf.Clamp01(-Input.GetAxisRaw("Horizontal"));
        //Debug.Log($"Left WheelSpeed:{leftWheelsVelocity} Right WheelSpeed: {rightWheelsVelocity}");
        foreach (HingeJoint joint in leftWheels)
        {
            joint.useMotor = true;
            var jointMotor = joint.motor;
            jointMotor.force = Mathf.Abs(Input.GetAxisRaw("Vertical") * leftWheelsForce);
            jointMotor.targetVelocity = 0;
                //rightWheelsVelocity * Mathf.Sign(Input.GetAxisRaw("Vertical"));
            joint.motor = jointMotor;
        }
        foreach (HingeJoint joint in rightWheels)
        {
            joint.useMotor = true;
            var jointMotor = joint.motor;
            jointMotor.force = Mathf.Abs(Input.GetAxisRaw("Vertical") * rightWheelsForce);
            jointMotor.targetVelocity = 0;
                //rightWheelsVelocity * Mathf.Sign(Input.GetAxisRaw("Vertical"));
            joint.motor = jointMotor;
        }
        
        foreach (HingeJoint joint in leftArms)
        {
            if (joint)
            {
                JointSpring jointSpring = joint.spring;
                jointSpring.targetPosition = Mathf.Lerp(jointSpring.targetPosition, -steeringArmAngle * Mathf.Clamp01(Input.GetAxisRaw("Horizontal")), steeringLerpSpeed);
                joint.spring = jointSpring;
            }
        }
        foreach (HingeJoint joint in rightArms)
        {
            if (joint)
            {
                JointSpring jointSpring = joint.spring;
                jointSpring.targetPosition = Mathf.Lerp(jointSpring.targetPosition, -steeringArmAngle * Mathf.Clamp01(-Input.GetAxisRaw("Horizontal")), steeringLerpSpeed);
                joint.spring = jointSpring;
            }
        }
    }
}
