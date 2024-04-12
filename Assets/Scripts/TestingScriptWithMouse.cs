using UnityEngine;

public class TestingScriptWithMouse : MonoBehaviour
{
    public float altitude = 12f;

    public GameObject earthCollider;
    void Start()
    {
        // for (int i = -180; i < 180; i++)
        // {
        //     // Note that the latitude must be inversed to be computed correctly
        //     // Calculate the position on the Earth's surface based on latitude and longitude
        //     Vector3 position = CalculatePositionOnEarth(30, i);
        // 
        //     // Create a sphere at the calculated position
        //     GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //     sphere.transform.parent = earthCollider.transform;
        //     sphere.transform.position = earthCollider.transform.position + position;
        // }
        Vector3 position = CalculatRelativePosition(37.86007f, 15.28948f);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.parent = earthCollider.transform;
        sphere.transform.localScale *= .7f;
        sphere.transform.position = earthCollider.transform.position + position;

        
        Debug.Log($"Position :{position} ");
        var lat_lon = GetLatLonFromPosition(position, altitude);
        Debug.Log($"Location: {lat_lon}");
        Debug.Log($"Position Really: {CalculatRelativePosition(lat_lon.x, lat_lon.y)}");


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
        Vector3 position = earthCollider.transform.rotation * new Vector3(x, y, z) * altitude;

        return position;
    }

    Vector2 GetLatLonFromPosition(Vector3 relativePosition, float altitude)
    {
        // Undo the scaling to the radius of the Earth
        relativePosition /= altitude;

        // Get the rotation of the Earth object
        Quaternion earthRotation = earthCollider.transform.rotation;

        // Undo the rotation of the Earth
        relativePosition = Quaternion.Inverse(earthRotation) * relativePosition;

        // Calculate latitude and longitude
        float latitude = Mathf.Asin(relativePosition.y) * Mathf.Rad2Deg;
        float longitude = Mathf.Atan2(relativePosition.z, relativePosition.x) * Mathf.Rad2Deg;

        return new Vector2(latitude, longitude);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with any object
            if (Physics.Raycast(ray, out hit))
            {

                Vector3 relativePosition = hit.point - earthCollider.transform.position;

                Debug.Log(relativePosition);

                var altitude = Vector3.Distance(hit.point, earthCollider.transform.position);
                // Calculate the latitude and longitude
                float latitude = GetLatLonFromPosition(relativePosition, altitude).x;
                float longitude = GetLatLonFromPosition(relativePosition, altitude).y;

                Debug.Log($"{name} Dropped at: Latitude: " + latitude + ", Longitude: " + longitude);

                // do scoring stuff afterwards

                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.parent = earthCollider.transform;
                go.transform.position = earthCollider.transform.position + relativePosition;
            }
        }
    }
}


