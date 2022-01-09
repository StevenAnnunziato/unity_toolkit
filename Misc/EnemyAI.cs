/*
 * 
 * This is a starter for AI with Unity's NavMesh system.
 * Child classes can call InitializeAI() in the Start() method, and
 * UpdateAI() in the update method. From there, unique behavior patterns can
 * build upon.
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Customizable Behaviour Properties")]
    public float randomOffsetRadius = 10f;
    public float homeDistance = 3f;
    public float passbyLockonDistance = 5f;
    public bool erraticMovement = false;

    [Header("Debug Visualizations")]
    public bool visualizeDestination = false;
    public GameObject testVis;

    // restricted member variables
    protected GameObject player;
    private NavMeshAgent nma;
    private Vector2 myOffset;
    private Vector3 targetPos;
    private GameObject debugVisual;
    // sentinel values
    private bool homing = false;
    private bool canFindNewOffset = true;
    private bool homingToNewOffset = false;

    protected void InitializeAI()
    {
        // get object references
        player = GameObject.FindGameObjectWithTag("Player");
        nma = GetComponent<NavMeshAgent>();

        nma.avoidancePriority = Random.Range(10, 40);

        FindNewOffset();

        // test
        if (visualizeDestination)
            debugVisual = Instantiate(testVis);
    }

    protected void UpdateAI()
    {
        targetPos = player.transform.position + new Vector3(myOffset.x, 0f, myOffset.y);

        if (!nma.isStopped)
        {
            if (!erraticMovement)
                nma.destination = FindDestination();
            else
                nma.destination = FindDestinationErratic();
        }
            

        // visualize destination
        if (visualizeDestination)
            debugVisual.transform.position = nma.destination;
    }

    // for giving or receiving attacks
    public void Stun(float dur)
    {
        StartCoroutine(Pause(dur));
    }

    private IEnumerator Pause(float dur)
    {
        nma.isStopped = true;
        yield return new WaitForSeconds(dur);
        nma.isStopped = false;
    }

    // helper function for FindDestination()
    // returns position of current destination
    private Vector3 WaypointPositionOnNavmesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        Debug.LogWarning("NavMeshHit could not be found");
        return player.transform.position;
    }

    // Returns the current destination to move to.
    // Includes methods of splitting up to create more interesting AI.
    protected Vector3 FindDestination()
    {
        // move directly to player
        if (homing)
            return player.transform.position;

        // calculate distances
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distToWaypoint = Vector3.Distance(transform.position, targetPos);
        // home directly to player
        if (distToWaypoint < homeDistance || distToPlayer < passbyLockonDistance)
        {
            homing = true;
            return player.transform.position;
        }
        // still far from target, so move to the random waypoint
        // this causes enemies to split up and corner the player
        else
        {
            return WaypointPositionOnNavmesh();
        }
    }

    // Returns the current destination to move to.
    // Includes methods of splitting up to create more interesting AI.
    // This method differs by dancing around the player and staying in motion.
    // This is better for faster enemies.
    protected Vector3 FindDestinationErratic()
    {

        float distToWaypoint = Vector3.Distance(transform.position, targetPos);

        // still far from target, so move to the random waypoint
        // this causes enemies to split up and corner the player
        if (!homing && distToWaypoint > homeDistance)
        {
            // move towards waypoint
            return WaypointPositionOnNavmesh();
        }
        else // Reached waypoint, in player's combat radius, within randomOffsetRadius units from player
        {
            // here, homing means the entity will dance around the player until killed
            homing = true;

            // going to new waypoint
            if (homingToNewOffset)
            {
                // reached new waypoint, go to player now
                if (distToWaypoint < homeDistance)
                {
                    homingToNewOffset = false;
                    canFindNewOffset = true;
                    return player.transform.position;
                }
                // waypoint still not reached, go there
                else
                {
                    return WaypointPositionOnNavmesh();
                }
            }
            // going to player
            else
            {
                float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
                // not yet at player
                if (distToPlayer > homeDistance)
                {
                    return player.transform.position;
                }
                // right next to player
                else
                {
                    // find a new destination and go there
                    if (canFindNewOffset)
                    {
                        FindNewOffset();
                        canFindNewOffset = false;
                    }

                    homingToNewOffset = true;

                    return WaypointPositionOnNavmesh();
                }
            }
        }
    }

    private void FindNewOffset()
    {
        myOffset = Random.insideUnitCircle.normalized * randomOffsetRadius;
    }

}
