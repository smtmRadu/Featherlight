using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [SerializeField] private Transform sun;
    [SerializeField] private float rotateAroundEarthSpeed = 1f;
    [SerializeField] private float rotateAroundSelfAxis = 1f;
    private void FixedUpdate()
    {
        this.transform.RotateAround(sun.position, Vector3.up, rotateAroundEarthSpeed);

        Vector3 axis = new Vector3(0.0f, 1f, 0.4f).normalized;
        this.transform.Rotate(transform.position + transform.rotation * axis, rotateAroundSelfAxis);
    }
}


