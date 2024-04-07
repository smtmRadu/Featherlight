using UnityEngine;

namespace kbradu
{
    public class PropellerRotationScript : MonoBehaviour
    {
        [SerializeField] private PlaneController planeController;
        [SerializeField] private float rotationSpeed = 20f;

        private void FixedUpdate()
        {
            transform.Rotate(Vector3.right, rotationSpeed * planeController.CurrentForwardSpeed);
        }
    }


}


