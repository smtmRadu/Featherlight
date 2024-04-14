using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public Shader fadeShader;

        
        private void Awake()
        {
            earthParent = transform.parent;
            destination = earthParent.GetComponent<CountriesDatabase>().GetRandomCountry();

            // Only for fading on collision;
            materialsCopy = new();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var materials = child.GetComponent<Renderer>().materials;
                var materialsCopyx = materials.Select(x =>
                {
                    var cop = new Material(x);
                    cop.name = "Some copied material here";
                    cop.SetFloat("_Mode", 2); // 2 corresponds to fade mode
                    cop.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    cop.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    // // cop.SetInt("_ZWrite", 0); // THIS FUCKS UP THE OBJECT AND APPEARS BUGGY
                    cop.DisableKeyword("_ALPHATEST_ON");
                    cop.DisableKeyword("_ALPHABLEND_ON");
                    cop.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    cop.renderQueue = 3000;

                    return cop;
                });
                child.GetComponent<Renderer>().materials = materialsCopyx.ToArray();
                materialsCopy.AddRange(materialsCopyx);
            }
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

        List<Material> materialsCopy;

        IEnumerator FadeIn()
        {
            // Record the starting time of the fade
            float startTime = Time.time;

            while (Time.time - startTime < fadeDuration)
            {
                float progress = (Time.time - startTime) / fadeDuration;

                int index = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    
                    int materialsInThisChild = child.GetComponent<Renderer>().materials.Length;

                    for (int k = 0; k < materialsInThisChild; k++)
                    {
                        float a = 1f - Mathf.Lerp(0f, 1f, progress);
                        Color oldColor = materialsCopy[index + k].GetColor("_Color");
                        materialsCopy[index + k].SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, a));

                    }

                    child.GetComponent<Renderer>().materials = materialsCopy.GetRange(index, materialsInThisChild).ToArray(); // this is actually needed....

                    index += materialsInThisChild;                  
                }
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }

}



