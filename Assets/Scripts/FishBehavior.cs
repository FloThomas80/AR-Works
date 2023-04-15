using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{

    public float Speed = 0.5f;
    float rotationSpeed = 2.0f;
    [SerializeField]
    private GameObject Bocal;
    Vector3 AveHeading;
    Vector3 AvePosition;
    Vector3 BocalPos;
    //public BoidsFish BoidsFishes;
    public float NeighbDistance = 5.0f;//a quelle distance ils se groupent pour faire un banc

    bool Turning = false;
    // Start is called before the first frame update
    void Start()
    {
        Bocal = GameObject.Find("Bocal");
        Speed = Random.Range(0.5f,1f);
        BocalPos = Vector3.zero;
        BocalPos = Bocal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, BocalPos) >= BoidsFish.TankSize) //si il est plus loin que la taille du bocal 
        {
            Turning = true;// il tourne
        }
        else Turning = false;

        if (Turning) // si il tourne :
        {
            Vector3 direction = Bocal.transform.position - transform.position; // fait demi tour
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            Speed = Random.Range(0.5f, 1f);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * Speed);
        
    }

    private void ApplyRules()
    {
        //Debug.Log("");
        GameObject[] gos;
        gos = BoidsFish.AllFish;

        Vector3 Vcenter = Vector3.zero;
        Vector3 Vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = BoidsFish.GoalPos;

        float dist;
        int groupSize = 0;

        foreach (GameObject go in gos)// pour chaque poisson dans le banc
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= NeighbDistance)
                {
                    Vcenter += go.transform.position;
                    groupSize++;

                    if(dist < 0.1f)
                    {
                        Vavoid = Vavoid + (this.transform.position - go.transform.position);
                    }

                    FishBehavior anotherFlock = go.GetComponent<FishBehavior>();
                    gSpeed = gSpeed + anotherFlock.Speed;
                }
            }
        }
        if(groupSize > 1)
        {
            Vcenter = Vcenter / groupSize + (goalPos - this.transform.position);
            Speed = gSpeed / groupSize;

            Vector3 direction = (Vcenter + Vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
