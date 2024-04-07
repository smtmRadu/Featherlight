using System.Net;
using TreeEditor;
using UnityEngine;

namespace kbradu
{
    public class PlaneController : MonoBehaviour
    {
        public float CurrentForwardSpeed { get => forwardSpeed; }

        [SerializeField] private GameObject earth;
        
        [SerializeField] private float baseForwardSpeed = 1f;
        [SerializeField] private float maxForwardSpeed = 6f;
        [SerializeField] private float maxRotationSpeed = 45f;
        [SerializeField] private float forwardAcceleration = 1.05f;
        [SerializeField] private float forwardDeceleration = 0.99f;
        [SerializeField] private float sidewaysAcceleration = 200f;
        [SerializeField] private float sidewaysDeceleration = 0.94f;

        [ViewOnly, SerializeField] private float forwardSpeed = 0.1f; // the actual forward speed
        [ViewOnly, SerializeField] private float sidewaysRotationSpeed = 0;
        private void FixedUpdate()
        {
            HandleInput();
            ConstantForwardMove();
            MoveSideways();
            Roll();
        }

        private void HandleInput()
        {
            // handle forward

            if (forwardSpeed < baseForwardSpeed)
                forwardSpeed = forwardSpeed * forwardAcceleration;
            else
            {
                if (Input.GetKey(KeyCode.W))
                    forwardSpeed = Mathf.Clamp(forwardSpeed * forwardAcceleration, baseForwardSpeed, maxForwardSpeed);
                else
                    forwardSpeed = Mathf.Clamp(forwardSpeed * forwardDeceleration, baseForwardSpeed, maxForwardSpeed);


            }

            // handle sideways
            if (Input.GetKey(KeyCode.A))
            {
                // Increase rotation speed to the left
                sidewaysRotationSpeed -= sidewaysAcceleration * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // Increase rotation speed to the right
                sidewaysRotationSpeed += sidewaysAcceleration * Time.fixedDeltaTime;
            }
            else
            {
                // Gradually slow down rotation when no keys are pressed
                sidewaysRotationSpeed *= sidewaysDeceleration;
            }

            // Clamp the rotation speed to within the maximum limits
            sidewaysRotationSpeed = Mathf.Clamp(sidewaysRotationSpeed, -maxRotationSpeed, maxRotationSpeed);
        }
        private void ConstantForwardMove()
        {
            Vector3 directionToPlane = (transform.position - earth.transform.position).normalized;
            Vector3 planeRotationAxisAroundEarth = Vector3.Cross(transform.right, directionToPlane);
            transform.RotateAround(earth.transform.position, planeRotationAxisAroundEarth, -forwardSpeed * Time.fixedDeltaTime);
        }

        private void MoveSideways()
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * sidewaysRotationSpeed);
        }
        private void Roll()
        {
           // // Calculate the roll angle based on the sideways rotation speed
           // float rollAngle = sidewaysRotationSpeed / 10f;
           // 
           // // Create a quaternion for the roll rotation
           // Quaternion rollRotation = Quaternion.Euler(rollAngle, 0, 0);
           // 
           // // Apply the roll rotation to the plane
           // transform.localRotation = rollRotation * transform.localRotation;
        }
        private void OnDrawGizmos()
        {
           
        }
    }
}


