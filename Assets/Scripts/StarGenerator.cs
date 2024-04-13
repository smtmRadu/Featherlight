using UnityEngine;

namespace kbradu
{
    public class StarGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private int starCount;
        [SerializeField] private Vector2 starScale;
        [SerializeField] private float distance;

        private void Start()
        {
            for (int i = 0; i < starCount; i++)
            {
                Vector3 starPos = Random.onUnitSphere * distance;

                var newStar = Instantiate(starPrefab, starPos, Quaternion.identity, transform);
                
                float scale = Random.Range(starScale.x, starScale.y);
                newStar.transform.localScale *= scale;
            }
        }
    }
}



