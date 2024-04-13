using Unity.VisualScripting;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
	[SerializeField] private AudioClip backgroundClip;
    [SerializeField] private float volume = 0.3f;


	private AudioSource audioSource;

    private void Awake()
    {
        if(backgroundClip != null)
        {
            audioSource = transform.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = backgroundClip;
            audioSource.playOnAwake = false;
            audioSource.volume = volume;
            audioSource.Play();
        }
       
    }
}


