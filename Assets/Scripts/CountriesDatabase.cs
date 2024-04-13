using System.Collections.Generic;
using UnityEngine;

namespace kbradu
{
    public class CountriesDatabase : MonoBehaviour
    {
        private Dictionary<string, Country> countryDict;
        public List<Country> countryList;
        private void Awake()
        {
            countryDict = new();
            countryList = new();
            TextAsset cititesCSV = Resources.Load<TextAsset>("Countries/countries");

            string[] data = cititesCSV.text.Split(new char[] { '\n' });

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });

                string name = row[0];
                string capital = row[1];
                float lat = 0f;
                try
                {
                    lat = float.Parse(row[2]);
                }
                catch { Debug.Log(row[2]); }
                float lon = float.Parse(row[3]);    
                int population = int.Parse(row[4]);
                string capType = row[5];

                Country c = new Country(name, capital, lat, lon, population, capType);

                countryDict.Add(name, c);
                countryList.Add(c);
            }

            Debug.Log($"Countries loaded = {countryDict.Count}");
        }

        public Country GetRandomCountry() => countryList[Random.Range(0, countryList.Count)];
    }


    public class Country
    {
        public string name;
        public string capitalCity;
        public float latitude;
        public float longitude;
        public int population;
        public string capitalType;

        public Country(string name, string capitalCity, float latitude, float longitude, int population, string capitalType)
        {
            this.name = name;
            this.capitalCity = capitalCity;
            this.latitude = latitude;
            this.longitude = longitude;
            this.population = population;
            this.capitalType = capitalType;
        }
    }

}


