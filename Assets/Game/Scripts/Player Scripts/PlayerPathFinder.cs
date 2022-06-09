using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPathFinder : MonoBehaviour
{
    private NavMeshAgent navAgent;
    
    public Transform[] navPoints;
    private int destinationIndex;

    private Vector3 currentDestination;

     void Awake()
    {
        
        navAgent = GetComponent<NavMeshAgent>();
        destinationIndex = 0;
        
        GoToNextDestination();


    }
    void Update()
    {
        
        CheckIfAgentReachedDestination();
    }
  
    void GoToNextDestination() {
        if (navPoints.Length == 0)
            return;
        //Debug.Log("Destination Points :" + navPoints[destinationIndex].position.x + navPoints[destinationIndex].position.y + navPoints[destinationIndex].position.z);
        if (destinationIndex >= navPoints.Length)
        {
            navAgent.Stop();
        }
        else { 
            navAgent.destination=(navPoints[destinationIndex].position);
            destinationIndex++;
        }
        


    }
    void CheckIfAgentReachedDestination() {

        if (!navAgent.pathPending && navAgent.remainingDistance < 1f) {
            Debug.Log("Destination ýndex :" + destinationIndex);
            
            GoToNextDestination();
        }
    }

    }//class









    





