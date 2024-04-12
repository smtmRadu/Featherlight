using kbradu;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlaneDropMechanism : MonoBehaviour
{
	[SerializeField] private GameObject dropPrefab;
    [SerializeField] private TMPro.TMP_Text uiDestinations;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private int depositCapacity = 3;

    List<Country> destinationsList = new List<Country>();
    public Country currentSelectedDestination = null;

    private void Update()
    {
        SwapBetweenDrops();
        if (Input.GetKeyDown(KeyCode.Space) && destinationsList.Count > 0)
            Drop();

        UpdateUI();

    }


    public bool CanDepositNewPackage() => destinationsList.Count < depositCapacity;

    private void SwapBetweenDrops()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var index = destinationsList.IndexOf(currentSelectedDestination);
            if (index > 0)
            {
                currentSelectedDestination = destinationsList[index - 1];
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var index = destinationsList.IndexOf(currentSelectedDestination);
            if (index < destinationsList.Count - 1)
            {
                currentSelectedDestination = destinationsList[index + 1];
            }
        }
    }
    public void DepositDrop(Country country)
    {
        Debug.Log($"Drop deposited ({country.name})");
        if (destinationsList.Count == 0)
            currentSelectedDestination = country;

        destinationsList.Add(country);
    }

    private void Drop()
    {
        var drop = Instantiate(dropPrefab, dropPoint.position, Quaternion.identity, transform.parent);
        drop.GetComponent<DropScript>().country = currentSelectedDestination;

        destinationsList.Remove(currentSelectedDestination);

        if(destinationsList.Count > 0)
            currentSelectedDestination = destinationsList[0];
    }

    private void UpdateUI()
    {
        StringBuilder sb = new StringBuilder(destinationsList.Count == 0 ? "You have no packages to deliver" : "");
        for (int i = 0; i < destinationsList.Count; i++)
        {
            
            var country = destinationsList[i];

            if (country == currentSelectedDestination)
                sb.Append("<color=green><b>");

            sb.Append(country.name);
            sb.Append(", ");
            sb.Append(country.capitalCity);

            if (country == currentSelectedDestination)
                sb.Append("</color></b>");

            if (i != destinationsList.Count - 1)
                sb.Append("     ");
        }
        uiDestinations.text = sb.ToString();
    }
   


}


