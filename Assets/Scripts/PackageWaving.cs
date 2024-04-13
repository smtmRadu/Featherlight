using UnityEngine;

namespace kbradu
{
    public class PackageWaving : MonoBehaviour
    {
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = 1f;


        private Vector3 initialPosition;
        private float randomBiasVertical;
        private float randomBiasHorizontal;

        private void Start()
        {
            initialPosition = transform.localPosition;
            randomBiasVertical = Random.value * Mathf.PI;
            randomBiasHorizontal = Random.value * Mathf.PI;
        }

        private void Update()
        {
            transform.localPosition = initialPosition + transform.right * Mathf.Cos((Time.time + randomBiasVertical) * frequency) * amplitude; // this is for up down (i know is right but it is actually for up down
            transform.localPosition = initialPosition + transform.up * Mathf.Cos((Time.time + randomBiasHorizontal) * frequency) * amplitude; // and viceversa
        }
    }



}

