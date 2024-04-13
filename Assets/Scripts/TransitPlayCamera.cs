using System.Collections;
using UnityEngine;

namespace kbradu
{
    public class TransitPlayCamera : MonoBehaviour
    {
        [SerializeField] private Camera menuCamera;
        [SerializeField] private float duration = 2f;

        private Transform target;
        public void Transit()
        {
            target = new GameObject("Temporary GO for transit play camera").transform;
            target.parent = transform.parent;
            target.position = transform.position;
            target.rotation = transform.rotation;

            transform.position = menuCamera.transform.position;
            transform.rotation = menuCamera.transform.rotation;
            StartCoroutine("TransitProcess");
        }

        IEnumerator TransitProcess()
        {
            float progress = 0f;

            while(progress < duration)
            {
                float smoothedProgress = progress / duration;
                smoothedProgress = Mathf.Sin(smoothedProgress * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(menuCamera.transform.position, target.position, smoothedProgress);
                transform.rotation = Quaternion.Lerp(menuCamera.transform.rotation, target.rotation, smoothedProgress);
                yield return new WaitForSeconds(Time.deltaTime);
                progress += Time.deltaTime;
            }

            transform.position = target.position;
            transform.rotation = target.rotation;
            Destroy(target.gameObject);
        }
    }
}



