using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreedUIController : MonoBehaviour
{

    public static BreedUIController Instance
    {
        get;
        private set;
    }

    public EvolutionController EvolutionController;

    public GameObject BreedMenu;
    public Button AutoBreedButton;
    public Button ManualBreedButton;
    public Slider MutationSlider;

    private int runnerLayer;

    void Awake()
    {
        Instance = this;

        AutoBreedButton.onClick.AddListener(StartAutoBreed);
        ManualBreedButton.onClick.AddListener(StartManualBreed);


        BreedMenu.SetActive(false);

        runnerLayer = LayerMask.NameToLayer("Runner");
    }

    public void Show()
    {
        BreedMenu.SetActive(true);
    }

    public void Hide()
    {
        BreedMenu.SetActive(false);
    }

    private void StartAutoBreed()
    {
        EvolutionController.AutoRepopulate();
        Hide();
    }

    private void StartManualBreed()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit = new RaycastHit();
            bool hitOccured = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, 1 << runnerLayer);
            if (hitOccured)
            {
                Debug.Log("Selected runner: " + hit.collider.gameObject);
            }
            else
            {
                //Debug.Log("No hit");
            }
        }


    }

}
