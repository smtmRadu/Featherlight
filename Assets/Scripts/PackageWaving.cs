using UnityEngine;

namespace kbradu
{
    public class PackageWaving : MonoBehaviour
    {
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = 1f;


        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = initialPosition + transform.up * Mathf.Cos(Time.time * frequency) * amplitude;
        }
    }



}

