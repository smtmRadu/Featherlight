using System.Collections.Generic;
using UnityEngine;

namespace kbradu
{
    public class PlayerScript : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text uiDropAccuracy;
        [SerializeField] private TMPro.TMP_Text statistics;
        [ViewOnly, SerializeField] private int score;

        LinkedList<(Country, DropAccuracyType, float)> deliveredDrops;

        private void Start()
        {
            deliveredDrops = new();
        }
        public void AppendMission(Country country, float error)
        {
            DropAccuracyType acc = Scoring.DropAccuracy(error);
            deliveredDrops.AddLast((country, acc, error));
            score += Scoring.Score(acc);

            statistics.text = $"Packages Delivered: {deliveredDrops.Count} \nScore: {score}";


            string text = "";

            switch(acc)
            {
                case DropAccuracyType.Perfect:
                    text = $"<color=green>Perfect deliver! You dropped the package right in <b>{country.name}</b>, <b>{country.capitalCity}</b>.</color>";
                    break;
                case DropAccuracyType.Good:
                    text = $"<color=yellow>Good deliver! You dropped the package <b>{(int)error}</b>km away from <b>{country.name}</b>, <b>{country.capitalCity}</b>.</color>";
                    break;
                case DropAccuracyType.Moderate:
                    text = $"<color=orange>Quite far from <b>{country.name}</b>, <b>{country.capitalCity}</b>! You dropped the package <b>{(int)error}</b>km away.</color>";
                    break;
                case DropAccuracyType.Bad:
                    text = $"<color=red>Oh no! You dropped the package so far from <b>{country.name}</b>, <b>{country.capitalCity}</b>.</color>";
                    break;
            }

            uiDropAccuracy.text = text;
        }
    }




    public static class Scoring
    {
        const float PERFECT_LIMIT = 100;
        const float GOOD_LIMIT = 500;
        const float MODERATE_LIMIT = 1000;

        public static DropAccuracyType DropAccuracy(float error_in_km)
        {
            switch (error_in_km)
            {
                case < PERFECT_LIMIT:
                    return DropAccuracyType.Perfect;
                case < GOOD_LIMIT:
                    return DropAccuracyType.Good;
                case < MODERATE_LIMIT:
                    return DropAccuracyType.Moderate;
                default:
                    return DropAccuracyType.Bad;
            }
        }

        public static int Score(DropAccuracyType type)
        {
            switch(type)
            {
                case DropAccuracyType.Perfect:
                    return 10;
                case DropAccuracyType.Good:
                    return 3;
                case DropAccuracyType.Moderate:
                    return 1;
                default: return 0;
            }
        }
    }

    public enum DropAccuracyType
    {
        Perfect,
        Good,
        Moderate,
        Bad
    }
}



