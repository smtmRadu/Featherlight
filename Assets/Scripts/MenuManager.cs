using UnityEngine;

namespace kbradu
{
    public class MenuManager : MonoBehaviour
    {

        [Header("Things to activate on Play")]
        [SerializeField] private PlaneController planeController;
        [SerializeField] private Camera planeCamera;
        [SerializeField] private ParticleSystem trailRenderer1;
        [SerializeField] private ParticleSystem trailRenderer2;
        [SerializeField] private GameObject playCanvas;

        [Header("Things to deactivate on Play")]
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private Camera menuCamera;



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }
        public void Play()
        {
            planeController.enabled = true;
            planeCamera.gameObject.SetActive(true);
            planeCamera.transform.GetComponent<TransitPlayCamera>().Transit();
            trailRenderer1.Play();
            trailRenderer2.Play();

            menuCanvas.SetActive(false);
            menuCamera.gameObject.SetActive(false);
            playCanvas.SetActive(true);
        }

        private void Pause()
        {
            planeController.enabled = false;
            planeCamera.gameObject.SetActive(false);
            trailRenderer1.Stop();
            trailRenderer2.Stop();

            menuCanvas.SetActive(true);
            menuCamera.gameObject.SetActive(true);
            playCanvas.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

}


