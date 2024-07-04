using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
public enum GarbageStatus { CollectingGarbage, DumpYard,TrashTrooper};
public class Vehcile : MonoBehaviour
{

    public NavMeshAgent agent;

    public List<Transform> waypoint1;
    public List<Transform> waypoint2;
    public List<Transform> waypoint3;

    public GarbageStatus garbageStatus;

    public Transform RayCastPoint;
    public float rayLength = 10.0f;
    public Vector3 boxHalfExtents = new Vector3(1.0f, 1.0f, 1.0f); // Half-extents of the box
    public bool DidHit;
    public bool IsPause;

    public Transform LastPoint;
    public bool NotReachedTarget;
    //public bool requestsend;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        VehcileSetDestination(GarbageStatus.CollectingGarbage);
    }
    public float Distancehere;
    void Update()
    {
        Distancehere=Vector3.Distance(transform.position, LastPoint.position);
        if(Vector3.Distance(transform.position, LastPoint.position)<=0.1f && NotReachedTarget)
        {
            //Debug.Log("Reached");
            NotReachedTarget = false;
        }
    }
    //public float rayLength = 10.0f; // Length of the box cast

    void FixedUpdate()
    {
        Vector3 fwd = RayCastPoint.TransformDirection(Vector3.forward);

        if (Physics.BoxCast(RayCastPoint.position, boxHalfExtents, fwd, out RaycastHit hit, RayCastPoint.rotation, rayLength))
        {
            //Debug.Log("There is something in front of the object!");
            if (hit.collider.gameObject.tag == "NPCVehcile" ||
                hit.collider.gameObject.tag == "TrafficPoint")
            {
                
                    DidHit = true;
                    PauseAgent();
                
            }
        }
        else
        {
            //Debug.Log("Did not Hit");
            DidHit = false;
            ResumeAgent();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "House":
                other.gameObject.GetComponent<House>().VehcileReached(this.gameObject);
                //StartCoroutine(other.gameObject.GetComponent<House>().waitVehcileReached(this.gameObject));
                break;
            case "DumpYard":
                other.transform.parent.gameObject.GetComponent<DumpYard>().VehcileReached(this.gameObject);
                break;
            case "TrashTrooper":
                other.gameObject.GetComponent<TrashTrooper>().VehcileReached(this.gameObject);
                break;
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 fwd = RayCastPoint.TransformDirection(Vector3.forward);

        // Draw the starting box of the cast
        Gizmos.matrix = Matrix4x4.TRS(RayCastPoint.position, RayCastPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

        // Draw the ending box of the cast
        Gizmos.matrix = Matrix4x4.TRS(RayCastPoint.position + fwd * rayLength, RayCastPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

        // Draw a line connecting the start and end points
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawLine(RayCastPoint.position, RayCastPoint.position + fwd * rayLength);
    }
    #region Last Time Use
    //void FixedUpdate()
    //{
    //    Vector3 fwd = RayCastPoint.TransformDirection(Vector3.forward);

    //    if (Physics.Raycast(RayCastPoint.position, fwd, rayLength))
    //    {
    //        Debug.Log("There is something in front of the object!");
    //        PauseAgent();
    //    }
    //    else
    //    {
    //        Debug.Log("Did not Hit");
    //        ResumeAgent();
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    // Draw a ray in the Scene view
    //    Vector3 fwd = RayCastPoint.TransformDirection(Vector3.forward);
    //    Gizmos.DrawRay(RayCastPoint.position, fwd * rayLength);
    //}
    #endregion
    public void VehcileSetDestination(GarbageStatus TempgarbageStatus, Transform Temptransform = null)
    {
        //requestsend = false;
        switch (TempgarbageStatus)
        {
            case GarbageStatus.CollectingGarbage:
                if (Temptransform != null)
                {
                    waypoint1.RemoveAt(waypoint1.Count - 1);
                    waypoint1.Add(Temptransform);
                }
                GoToDestination(waypoint1);
                //garbageStatus = TempgarbageStatus;
                break;
            case GarbageStatus.DumpYard:
                //garbageStatus = TempgarbageStatus;
                GoToDestination(waypoint2);
                break;
            case GarbageStatus.TrashTrooper:
                //garbageStatus = TempgarbageStatus;
                GoToDestination(waypoint3);
                break;
        }
    }
    async void GoToDestination(List<Transform> TempWayPoints)
    {
        bool TempshouldMove = true;
        int TempcurrentState = 0;
        LastPoint = TempWayPoints[TempWayPoints.Count - 1];
        NotReachedTarget = true;

        while (TempshouldMove == true)
        {

            if (TempWayPoints.Count > 0 && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                TempcurrentState = (TempcurrentState + 1) % TempWayPoints.Count;
                agent.SetDestination(TempWayPoints[TempcurrentState].position);
            }
            if (TempcurrentState == TempWayPoints.Count - 1)
            {
                TempshouldMove = false;
            }
            await Task.Yield();
        }
        
    }

    public void PauseAgent()
    {
        if (agent != null)
        {
            IsPause = true;
            agent.enabled = false;
            //agent.isStopped = true;
            //agent.speed = 0;
            //agent.angularSpeed = 0;
            //agent.acceleration = 0;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void ResumeAgent()
    {
        if (agent != null && LastPoint!=null && IsPause)
        {
            IsPause = false;
            agent.enabled = true;
            agent.SetDestination(LastPoint.position);
            //agent.isStopped = false;
            //agent.speed = 3.5f;
            //agent.angularSpeed = 120;
            //agent.acceleration = 8;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
    #region Dustbin
    //IEnumerator MoveToWaypoints(Transform[] TempWayPoints, int TempcurrentState)
    //{
    //    bool TempshouldMove=true;
    //    while (TempshouldMove==true)
    //    {
    //        if (TempWayPoints.Length > 0 && !agent.pathPending && agent.remainingDistance < 0.5f)
    //        {
    //            TempcurrentState = (TempcurrentState + 1) % TempWayPoints.Length;
    //            agent.SetDestination(TempWayPoints[TempcurrentState].position);
    //        }
    //        if (TempcurrentState == TempWayPoints.Length - 1)
    //        {
    //            TempshouldMove = false;
    //        }
    //        yield return null;
    //    }
    //}
    //private void Update()
    //{
    //    if (shouldMove && target1.Length > 0 && !agent.pathPending && agent.remainingDistance < 0.5f)
    //    {
    //        currentTargetIndex = (currentTargetIndex + 1) % target1.Length;
    //        agent.SetDestination(target1[currentTargetIndex].position);

    //        if (currentTargetIndex == target1.Length - 1)
    //        {
    //            shouldMove = false;
    //        }
    //    }
    //}
    #endregion
}
