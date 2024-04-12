using System.Collections.Generic;
using UnityEngine;

namespace kbradu
{
    public class PackageSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject packagePrefab;
        [SerializeField] private int noPackagesAtATime = 4;

        Dictionary<PackageScript, GameObject> packages;

        private void Start()
        {
            packages = new();

            for (int i = 0; i < noPackagesAtATime; i++)
                CreatePackage();


        }

        private void Update()
        {
            if (packages.Count < noPackagesAtATime)
                CreatePackage();
        }


        private void CreatePackage()
        {
            var newPack = Instantiate(packagePrefab, this.transform);

            var newPackScript = newPack.GetComponent<PackageScript>();
            newPackScript.onCatch += (package, e) =>
            {
                packages.Remove(package as PackageScript);
            };
            packages.Add(newPackScript, newPack);
        }
    }

}


