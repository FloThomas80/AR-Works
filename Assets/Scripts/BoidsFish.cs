using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsFish : MonoBehaviour
{
    public GameObject FishPrefab1;
    public GameObject GoalPrefab;
    public GameObject TankPrefab;
    public static int TankSize = 8;
    [SerializeField]
    static int NumbFish = 20;
    public static GameObject[] AllFish = new GameObject[NumbFish];

    float xx;
    float yy;
    float zz;

    public static Vector3 GoalPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        xx = transform.position.x;
        yy = transform.position.y;
        zz = transform.position.z;
        //TankPrefab = this.GetComponent<GameObject>();
        //Debug.Log(TankPrefab.name + "  re "+ TankPrefab);

        for (int i = 0;i < NumbFish;i++)
        {
            //xx = TankPrefab.transform.position.x + Random.Range(-TankSize, TankSize);
            //yy = TankPrefab.transform.position.y + Random.Range(-TankSize, TankSize);
            //zz = TankPrefab.transform.position.z + Random.Range(-TankSize, TankSize);

            Debug.Log(xx + "  " + yy + "  " + zz);
            Vector3 pos = new Vector3(xx, yy, zz);//on definit le spawn du poisson en fonction du centre du bocal et de sa taille
            AllFish[i] = (GameObject)Instantiate(FishPrefab1, pos, Quaternion.identity);
         
        }
              
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,1000) < 50)//toutes les combien de frame on change le point de goal des poissons
        {

            xx = transform.position.x + Random.Range(-TankSize, TankSize);
            yy = transform.position.y + Random.Range(-TankSize, TankSize);
            zz = transform.position.z + Random.Range(-TankSize, TankSize); 

            GoalPos = new Vector3(xx, yy, zz);//on definit la taille du bocal en fonction de son centre
            GoalPrefab.transform.position = GoalPos;
        }
    }
}
