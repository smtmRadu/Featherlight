using System;
using System.Collections;
using UnityEngine;

namespace kbradu
{
    public class PackageScript : MonoBehaviour
    {
        public float reachAltitude = 0.12f;
        public float fadeDuration = 1f;
        private Transform earthParent;

        public event EventHandler onCatch;
        public Country destination;
        public AnimationCurve animCurve;
        
        private void Awake()
        {
            earthParent = transform.parent;
            destination = earthParent.GetComponent<CountriesDatabase>().GetRandomCountry();
        }

        private void Start()
        {
            Vector3 randomPointOnSphere = UnityEngine.Random.onUnitSphere;
            transform.localPosition = randomPointOnSphere * reachAltitude;        
            transform.rotation = Quaternion.LookRotation((transform.position - earthParent.position).normalized) * Quaternion.Euler(90f, 0f, 0);
            float randomYRot = UnityEngine.Random.Range(0, 360f); // rotation is bad
            transform.Rotate(0, randomYRot, 0);
        }

        private void Update()
        {
            transform.Rotate(0, Time.deltaTime, 0); // little self rotation
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plane") && other.GetComponent<PlaneDropMechanism>().CanDepositNewPackage())
            {
               Debug.Log("Package catched");
               other.GetComponent<PlaneDropMechanism>().Deposit(this.destination);
               onCatch?.Invoke(this, EventArgs.Empty);
               GetComponent<BoxCollider>().enabled = false;
               StartCoroutine("FadeIn");      
            }

        }
        IEnumerator FadeIn()
        {
            // Record the starting time of the fade
            float startTime = Time.time;

            while (Time.time - startTime < fadeDuration)
            {
                float progress = (Time.time - startTime) / fadeDuration;
                progress = Mathf.Clamp(progress / 100f, 0f, 0.01f);
                var newScale = Vector3.one * animCurve.Evaluate(progress);
                transform.localScale = new Vector3(Mathf.Clamp01(newScale.x), Mathf.Clamp01(newScale.y), Mathf.Clamp01(newScale.z));
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }

}



