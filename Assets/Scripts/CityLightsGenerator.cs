using UnityEngine;

namespace kbradu
{
    public class CityLightsGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject cityLightPrefab;
        [SerializeField] private int num_lights = 1000;
        [SerializeField] private float altitude = 11.5f;
        private void Start()
        {
            var countriesDB = transform.parent.GetComponent<CountriesDatabase>();
            var list = countriesDB.countryList;

            foreach (var item in list)
            {
                Instantiate(cityLightPrefab, transform.parent.position + CalculatRelativePosition(item.latitude, item.longitude), Quaternion.identity, transform);
            }
        }



        Vector3 CalculatRelativePosition(float lat, float lon)
        {
            // Convert latitude and longitude from degrees to radians
            float latRad = lat * Mathf.Deg2Rad;
            float lonRad = lon * Mathf.Deg2Rad;

            // Calculate the position on the unit sphere
            float x = Mathf.Cos(latRad) * Mathf.Cos(lonRad);
            float y = Mathf.Sin(latRad);
            float z = Mathf.Cos(latRad) * Mathf.Sin(lonRad);

            // Scale the position to the radius of the Earth
            Vector3 position = transform.parent.rotation * new Vector3(x, y, z) * altitude;

            return position;
        }

    }

}


