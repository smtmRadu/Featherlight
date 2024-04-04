using UnityEngine;

namespace kbradu
{
    public class MoonRotation : MonoBehaviour
    {
        [SerializeField] private Transform earth;
        [SerializeField] private float rotateAroundEarthSpeed = 1f;
        [SerializeField] private float rotateAroundSelfAxis = 1f;
        private void FixedUpdate()
        {
            this.transform.RotateAround(earth.position, Vector3.up, rotateAroundEarthSpeed);

            Vector3 axis = new Vector3(0.1f, 1f, 0.4f).normalized;
            this.transform.Rotate(axis, rotateAroundSelfAxis);
        }

    }
}



