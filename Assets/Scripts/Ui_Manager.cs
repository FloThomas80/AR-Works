using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Manager : MonoBehaviour
{

    GameObject[] gos;

    [SerializeField]
    private BoidsFish BoidsFish;

    private void Start()
    {
        gos = BoidsFish.AllFish;
        //Clown = BoidsFish.FishPrefab1;
        //Angel = BoidsFish.FishPrefab2;
    }
    public void ChangeFishClown()
    {
        Debug.Log("Let's go Clown");
        for (int i = 0; i < gos.Length; i++)
        {
            gos[i].transform.GetChild(0).gameObject.SetActive(false);
            gos[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ChangeFishAngel()
    {
        Debug.Log("Let's go Angel");
        for (int i = 0; i < gos.Length; i++)
        {
            gos[i].transform.GetChild(0).gameObject.SetActive(true);
            gos[i].transform.GetChild(1).gameObject.SetActive(false);
        }

    }
}
