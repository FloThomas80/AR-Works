using UnityEngine;
using System.Collections.Generic;

public class FishController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float maxSteer = 0.5f;
    public float neighborDistance = 3f;
    public float separationDistance = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1f;
    public float obstacleAvoidanceWeight = 1f;
    public float obstacleAvoidanceDistance = 5f;
    public float wanderWeight = 1f;
    public float wanderRadius = 1f;
    public float wanderDistance = 2f;
    public float wanderJitter = 0.1f;
    public LayerMask obstacleLayer;

    private Rigidbody rb;
    private List<GameObject> neighbors;
    private Vector3 wanderTarget;
    private Vector3 boundsSize;
    private Vector3 boundsCenter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        neighbors = new List<GameObject>();
        wanderTarget = Random.insideUnitSphere * wanderRadius;

        // Get the size and center of the FishZone collider
        Collider zoneCollider = GameObject.Find("FishZone").GetComponent<Collider>();
        boundsSize = zoneCollider.bounds.size;
        boundsCenter = zoneCollider.bounds.center;
    }

    void FixedUpdate()
    {
        Vector3 separation = Separation();
        Vector3 alignment = Alignment();
        Vector3 cohesion = Cohesion();
        Vector3 obstacleAvoidance = ObstacleAvoidance();
        Vector3 wander = Wander();

        Vector3 steering = alignment * alignmentWeight
                            + cohesion * cohesionWeight
                            + separation * separationWeight
                            + obstacleAvoidance * obstacleAvoidanceWeight
                            + wander * wanderWeight;

        steering = Vector3.ClampMagnitude(steering, maxSteer);
        Vector3 velocity = rb.velocity + steering;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        rb.velocity = velocity;

        if (velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity.normalized), Time.deltaTime * 5f);
        }

        // Keep the fish inside the FishZone collider
        Vector3 newPos = transform.position + rb.velocity * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, boundsCenter.x - boundsSize.x / 2, boundsCenter.x + boundsSize.x / 2);
        newPos.y = Mathf.Clamp(newPos.y, boundsCenter.y - boundsSize.y / 2, boundsCenter.y + boundsSize.y / 2);
        newPos.z = Mathf.Clamp(newPos.z, boundsCenter.z - boundsSize.z / 2, boundsCenter.z + boundsSize.z / 2);
        transform.position = newPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            neighbors.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            neighbors.Remove(other.gameObject);
        }
    }

    Vector3 Alignment()
    {
        Vector3 alignment = Vector3.zero;
        if (neighbors.Count == 0)
        {
            return alignment;
        }

        foreach (GameObject neighbor in neighbors)
        {
            alignment += neighbor.GetComponent<Rigidbody>().velocity;
        }
        alignment /= neighbors.Count;

        return alignment.normalized;
    }

    Vector3 Cohesion()
    {
        Vector3 cohesion = Vector3.zero;
        if (neighbors.Count == 0)
        {
            return cohesion;
        }

        foreach (GameObject neighbor in neighbors)
        {
            cohesion += neighbor.transform.position;
        }
        cohesion /= neighbors.Count;

        return (cohesion - transform.position).normalized;
    }

    //
    Vector3 Separation()
    {
        Vector3 separation = Vector3.zero;
        if (neighbors.Count == 0)
        {
            return separation;
        }

        foreach (GameObject neighbor in neighbors)
        {
            Vector3 diff = transform.position - neighbor.transform.position;
            if (diff.magnitude < separationDistance)
            {
                separation += diff.normalized / diff.magnitude;
            }
        }

        return separation.normalized;
    }

    Vector3 ObstacleAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleAvoidanceDistance, obstacleLayer))
        {
            avoidance = Vector3.Cross(Vector3.up, hit.normal);
        }

        return avoidance.normalized;
    }

    Vector3 Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * wanderJitter;
        wanderTarget = wanderTarget.normalized * wanderRadius;

        Vector3 targetPos = transform.position + transform.forward * wanderDistance + wanderTarget;
        targetPos.y = transform.position.y;

        return (targetPos - transform.position).normalized;
    }
}