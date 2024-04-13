using UnityEngine;


namespace kbradu
{
    public class CloudScript : MonoBehaviour
    {
        public float reachAltitude = 0.12f;
        public float forwardSpeed = 10f;

        private void Start()
        {
            Vector3 randomPointOnSphere = UnityEngine.Random.onUnitSphere;
            transform.localPosition = randomPointOnSphere * reachAltitude; 
            transform.rotation = Quaternion.LookRotation((transform.position - transform.parent.position).normalized) * Quaternion.Euler(90f, 0f, 0);

            float randomRot = Random.Range(0, 360f);
            transform.Rotate(0, randomRot, 0);

        }

        private void Update()
        {
            Vector3 directionToPlane = (transform.position - transform.parent.position).normalized;
            Vector3 planeRotationAxisAroundEarth = Vector3.Cross(transform.right, directionToPlane);
            transform.RotateAround(transform.parent.position, planeRotationAxisAroundEarth, -forwardSpeed * Time.deltaTime);
        }
    }


}

