using UnityEngine;
using UnityEngine.UI;

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


        [Header("Mobile controls")]
        [SerializeField] private ButtonState acceleartion1;
        [SerializeField] private ButtonState acceleartion2;

        private void Start()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;
                // Enable both landscape orientations
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                // Disable both portrait orientations
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;

                if (SystemInfo.supportsGyroscope)
                {
                    Input.gyro.enabled = true;
                    Debug.Log("Gyroscope has been enabled.");
                }
                else
                {
                    Debug.Log("This device does not have a gyroscope.");
                }
            }
           

        }

        private void FixedUpdate()
        {
            HandleInputMobile();
            HandleInputDesktop();       
            ConstantForwardMove();
            MoveSideways();
            Roll();
        }

        private void HandleInputDesktop()
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
        private void HandleInputMobile()
        {
            if (!(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
                return;

            if(acceleartion1.IsPressed || acceleartion2.IsPressed)
                forwardSpeed = Mathf.Clamp(forwardSpeed * forwardAcceleration, baseForwardSpeed, maxForwardSpeed);
            else
                forwardSpeed = Mathf.Clamp(forwardSpeed * forwardDeceleration, baseForwardSpeed, maxForwardSpeed);



            var attitude = Input.gyro.attitude;
            attitude = new Quaternion(attitude.x, attitude.y, attitude.z, attitude.w);

            // Get the vector representing global up (away from gravity)
            // within the device's coordinate system.
            Vector3 localDown = Quaternion.Inverse(Input.gyro.attitude) * Vector3.down;

            // Extract our roll rotation - how much gravity points to our left or right.
            float rollDegrees = Mathf.Asin(localDown.x) * Mathf.Rad2Deg;

            // Extract our pitch rotation - how much gravity points forward or back.
            float pitchDegrees = Mathf.Atan2(localDown.y, localDown.z) * Mathf.Rad2Deg;

            print(pitchDegrees);

            // Now, you can use the attitude quaternion to control your object. For example:
            if (attitude.eulerAngles.z > 180 && attitude.eulerAngles.z < 360)
            {
                // Increase rotation speed to the left
                sidewaysRotationSpeed -= sidewaysAcceleration * Time.fixedDeltaTime;
            }
            else if (attitude.eulerAngles.z >= 0 && attitude.eulerAngles.z < 180)
            {
                // Increase rotation speed to the right
                sidewaysRotationSpeed += sidewaysAcceleration * Time.fixedDeltaTime;
            }
            else
            {
                // Gradually slow down rotation when no keys are pressed
                sidewaysRotationSpeed *= sidewaysDeceleration;
            }
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
    }
}


