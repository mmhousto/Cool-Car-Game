using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour
{

    public GameObject[] cars;
    public TextMeshProUGUI carName;
    private int currentCar = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carName.text = cars[0].name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCar()
    {
        Debug.Log("Selected Car: " + currentCar);
    }

    public void Next()
    {
        cars[currentCar].SetActive(false);
        currentCar++;
        if(currentCar >= cars.Length) currentCar = 0;
        cars[currentCar].SetActive(true);
        carName.text = cars[currentCar].name;
    }

    public void Previous()
    {
        cars[currentCar].SetActive(false);
        currentCar--;
        if (currentCar < 0) currentCar = cars.Length-1;
        cars[currentCar].SetActive(true);
        carName.text = cars[currentCar].name;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
