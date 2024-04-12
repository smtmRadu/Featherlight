using System;
using UnityEngine;

namespace kbradu
{
    public class PackageScript : MonoBehaviour
    {
        public float reachAltitude = 0.12f;
        private Transform earthParent;

        public event EventHandler onCatch;
        public Country destination;

        
        private void Awake()
        {
            earthParent = transform.parent;
            destination = earthParent.GetComponent<CountriesDatabase>().GetRandomCountry();
        }

        private void Start()
        {
            // Generate a random point on the surface of a sphere (representing Earth)
            Vector3 randomPointOnSphere = UnityEngine.Random.onUnitSphere;

            // Set the position of the package to the random point scaled by the desired altitude
            transform.localPosition = randomPointOnSphere * reachAltitude; // Adding Earth's radius to altitude

            // float randomYRot = UnityEngine.Random.Range(0, 360f); // rotation is bad
            // Align the package with the Earth's center
            transform.rotation = Quaternion.LookRotation((transform.position - earthParent.position).normalized) * Quaternion.Euler(90f, 0f, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plane") && other.GetComponent<PlaneDropMechanism>().CanDepositNewPackage())
            {
               Debug.Log("Package catched");
               other.GetComponent<PlaneDropMechanism>().DepositDrop(this.destination);
               onCatch?.Invoke(this, EventArgs.Empty);
               Destroy(this.gameObject);
            }

        }
    }

}



