using kbradu;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneDropMechanism : MonoBehaviour
{

    [SerializeField] private AudioClip dropSoundEffect;
	[SerializeField] private GameObject dropPrefab;
    [SerializeField] private TMPro.TMP_Text uiDestinations;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private int DEPOSIT_CAPACITY = 3;
    [SerializeField] private List<GameObject> dropsInside;

    List<Country> destinationsList = new List<Country>();
    public Country currentSelectedDestination = null;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = transform.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.pitch = 0.5f;
        audioSource.playOnAwake = false;
        audioSource.clip = dropSoundEffect;
    }

    private void Update()
    {
        SwapBetweenDrops();
        if (Input.GetKeyDown(KeyCode.Space))
            Drop();

        UpdateUI();

    }


    public bool CanDepositNewPackage() => destinationsList.Count < DEPOSIT_CAPACITY;

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
    public void SwapToLeftDrop()
    {
        var index = destinationsList.IndexOf(currentSelectedDestination);
        if (index > 0)
        {
            currentSelectedDestination = destinationsList[index - 1];
        }
    }

    public void SwapToRightDrop()
    {
        var index = destinationsList.IndexOf(currentSelectedDestination);
        if (index < destinationsList.Count - 1)
        {
            currentSelectedDestination = destinationsList[index + 1];
        }
    }
    public void Deposit(Country country)
    {
        Debug.Log($"Drop deposited ({country.name})");
        if (destinationsList.Count == 0)
            currentSelectedDestination = country;

        destinationsList.Add(country);

        foreach (var item in dropsInside)
        {
            if(!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void Drop()
    {
        if (!(destinationsList.Count > 0))
            return;

        var drop = Instantiate(dropPrefab, dropPoint.position, Quaternion.identity, transform.parent);
        drop.GetComponent<DropScript>().country = currentSelectedDestination;
        destinationsList.Remove(currentSelectedDestination);

        if(destinationsList.Count > 0)
            currentSelectedDestination = destinationsList[0];

        audioSource.Play();

        for (int i = dropsInside.Count - 1; i >= 0; i--)
        {
            var item = dropsInside[i];
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);
                break;
            }
            
        }
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


