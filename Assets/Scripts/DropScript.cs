using UnityEngine;

namespace kbradu
{
    public class DropScript : MonoBehaviour
    {
        [SerializeField] private float sizeDecrement = 0.995f;
        public Country country;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            transform.localScale *= sizeDecrement;


            Vector3 direction = transform.position - transform.parent.position;
            float distance = direction.magnitude;

            // Calculate the gravity force
            float forceMagnitude = 9.81f * rb.mass / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;

            // Apply the force to the object
            rb.AddForce(-force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("Earth"))
            {
                ContactPoint contactPoint = collision.contacts[0];
                GameObject earthCollider = transform.parent.Find("EarthCollider").gameObject;

                Vector3 relativePositionToEarth = contactPoint.point - earthCollider.transform.position;
                float altitude = Vector3.Distance(contactPoint.point, earthCollider.transform.position);
                Vector2 lat_long = GetLatLonFromPosition(relativePositionToEarth, earthCollider, altitude);

                float errorDistance = CalculateDropError(lat_long.x, lat_long.y, country.latitude, country.longitude);
                Debug.Log($"{name} - [Error: {errorDistance}km] | dropped at [Latitude: " + lat_long.x+ ", Longitude: " + lat_long.y + "]" + $" | target at [Latitude: {country.latitude}, Longitude: {country.longitude}]");

                // do scoring stuff afterwards here



                Destroy(this.gameObject);
            }
                
        }
        Vector2 GetLatLonFromPosition(Vector3 relativePositionToEarth, GameObject earthCollider, float altitude)
        {
            relativePositionToEarth /= altitude;

            // Get the rotation of the Earth object
            Quaternion earthRotation = earthCollider.transform.rotation;

            // Undo the rotation of the Earth
            relativePositionToEarth = Quaternion.Inverse(earthRotation) * relativePositionToEarth;

            // Calculate latitude and longitude
            float latitude = Mathf.Asin(relativePositionToEarth.y) * Mathf.Rad2Deg;
            float longitude = Mathf.Atan2(relativePositionToEarth.z, relativePositionToEarth.x) * Mathf.Rad2Deg;

            return new Vector2(latitude, longitude);
        }

        float CalculateDropError(float lat1, float long1, float lat2, float long2)
        {
            const float earthRadius = 6371f;
            float latDistance = (lat2 - lat1) * Mathf.Deg2Rad;
            float longDistance = (long2 - long1) * Mathf.Deg2Rad;

            float a = Mathf.Sin(latDistance / 2f) * Mathf.Sin(latDistance / 2f) + 
                Mathf.Cos(lat1 * Mathf.Deg2Rad)  * Mathf.Cos(lat2 * Mathf.Deg2Rad) *
                Mathf.Sin(longDistance / 2f) * Mathf.Sin(longDistance / 2f);

            float c= 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f - a));
            float distance = earthRadius * c;
            return distance;
        }
        
    }

}


