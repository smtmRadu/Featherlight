using UnityEngine;
using UnityEngine.UI;

namespace kbradu
{
    public class PlaneController : MonoBehaviour
    {
        public float CurrentForwardSpeed { get => forwardSpeed; }

        [SerializeField] private GameObject earth;
        [SerializeField] private GameObject MeshComponents;
        
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
        [SerializeField] private ButtonState rotateLeft;
        [SerializeField] private ButtonState rotateRight;

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
           
        }

        private void Update()
        {
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


            // handle sideways
            if (rotateLeft.IsPressed)
            {
                // Increase rotation speed to the left
                sidewaysRotationSpeed -= sidewaysAcceleration * Time.fixedDeltaTime;
            }
            else if (rotateRight.IsPressed)
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
            transform.Rotate(Vector3.up, Time.deltaTime * sidewaysRotationSpeed);
        }
        private void Roll()
        {
            if (Mathf.Abs(sidewaysRotationSpeed) > maxRotationSpeed * 0.97f)
            {
                // Roll the plane in the same direction and speed as the sideways rotation
                // if(MeshComponents.transform.localEulerAngles.x < 45f && MeshComponents.transform.localEulerAngles.x > -45f)
                    MeshComponents.transform.Rotate(Vector3.right, -sidewaysRotationSpeed * Time.deltaTime);
            }
            else
            {
                // Gradually reduce the roll of the plane back to zero when the sideways rotation stops
                float xRotation = MeshComponents.transform.localEulerAngles.x;
                xRotation = (xRotation > 180) ? xRotation - 360 : xRotation; // Convert angle to [-180, 180] range
                MeshComponents.transform.Rotate(Vector3.right, -xRotation * sidewaysDeceleration * Time.deltaTime);
            }
        }
    }
}


