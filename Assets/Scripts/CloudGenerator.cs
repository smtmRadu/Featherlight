using System.Collections.Generic;
using UnityEngine;

namespace kbradu
{
    public class CloudGenerator : MonoBehaviour
    {
        [SerializeField] int cloudsNumber = 30;
        [SerializeField] List<GameObject> cloudsPrefabs;
        private void Start()
        {
            for (int i = 0; i < cloudsNumber; i++)
            {
                var randIndex = Random.Range(0, cloudsPrefabs.Count);
                var randomCloud = cloudsPrefabs[randIndex];
                Instantiate(randomCloud, transform);
            }


        }
    }

}


