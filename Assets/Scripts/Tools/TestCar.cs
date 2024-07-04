using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestCar : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] target1;
    public int currentTargetIndex = 0;
    public bool shouldMove = false;
    public Transform target2;
    public Transform target3;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(target1.Length);
            shouldMove = true;
            if (target1.Length > 0)
            {
                agent.SetDestination(target1[currentTargetIndex].position);
            }
            //if (target1.Length > 0)
            //{


            //agent.SetDestination(target1[currentTargetIndex].position);
            //currentTargetIndex = (currentTargetIndex + 1) % target1.Length; // Move to the next target, wrap around if necessary
            //}
        }
        if (shouldMove && target1.Length > 0 && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % target1.Length;
            agent.SetDestination(target1[currentTargetIndex].position);
            
            if (currentTargetIndex == target1.Length-1)
            {
                shouldMove = false;
            }
        }



        if (Input.GetKeyDown(KeyCode.B))
        {
            agent.SetDestination(target2.position);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            agent.SetDestination(target3.position);
        }



        
    }

}
