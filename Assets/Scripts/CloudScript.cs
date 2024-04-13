using System.Collections.Generic;
using UnityEngine;


namespace kbradu
{
    public class CloudScript : MonoBehaviour
    {
        public float reachAltitude = 0.12f;
        public float forwardSpeed = 0.75f;


        [SerializeField] float dissolveSpeed = 0.7f;
        [SerializeField] List<GameObject> cloudsPrefabs;

        private Material selfMaterial;
        private Renderer renderer;

        private void Start()
        {
            Vector3 randomPointOnSphere = UnityEngine.Random.onUnitSphere;
            transform.localPosition = randomPointOnSphere * reachAltitude; 
            transform.rotation = Quaternion.LookRotation((transform.position - transform.parent.position).normalized) * Quaternion.Euler(90f, 0f, 0);

            float randomRot = Random.Range(0, 360f);
            transform.Rotate(0, randomRot, 0);

            renderer = GetComponent<Renderer>();
            selfMaterial = new Material(renderer.material);
            selfMaterial.name = "Some copied material here";
            selfMaterial.SetFloat("_Mode", 2); // 2 corresponds to fade mode
            selfMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            selfMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            selfMaterial.DisableKeyword("_ALPHATEST_ON");
            selfMaterial.DisableKeyword("_ALPHABLEND_ON");
            selfMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            selfMaterial.renderQueue = 3000;

        }

        private void Update()
        {
            Vector3 directionToPlane = (transform.position - transform.parent.position).normalized;
            Vector3 planeRotationAxisAroundEarth = Vector3.Cross(transform.right, directionToPlane);
            transform.RotateAround(transform.parent.position, planeRotationAxisAroundEarth, -forwardSpeed * Time.deltaTime);

            // Regain color back
            if(renderer.material.color.a < 1f)
            {
                float newAlpha = selfMaterial.color.a + dissolveSpeed * Time.deltaTime / 10f;
                newAlpha = Mathf.Clamp01(newAlpha);
                selfMaterial.color = new Color(selfMaterial.color.r, selfMaterial.color.g, selfMaterial.color.b, newAlpha);
                renderer.material = selfMaterial;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("here");
            if(other.CompareTag("Plane"))
            {
                float newAlpha = selfMaterial.color.a - dissolveSpeed * Time.deltaTime;
                newAlpha = Mathf.Clamp01(newAlpha);
                selfMaterial.color = new Color(selfMaterial.color.r, selfMaterial.color.g, selfMaterial.color.b, newAlpha);
                renderer.material = selfMaterial;
            }
        }
    }


}

