using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Physics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Random = System.Random;

public class ShooterAI : MonoBehaviour
{
    private static float HEIGHT_MIX = 50;

    public float wanderTime = 6;
    public int maxWanderDistance = 1400;
    private float timer;

    [SerializeField]
    private NavMeshAgent agent;


    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject playerBody;

    [SerializeField]
    private Vector3 rayOffset = new Vector3(0, 30, 0);

    public float maxShootingDistance = Mathf.Infinity;

    void OnEnable() {
        timer = wanderTime;
    }

    void OnTriggerEnter(Collider collider) {
        agent.isStopped = true;
        Debug.Log("collided!");
    }

    void FixedUpdate() {
        //maybe try just dropping random points in the navmesh initially and choosing a random one to patrol to?

        timer += Time.deltaTime;

        Vector3[] angles = new[] {
            transform.forward,
            Quaternion.AngleAxis(45, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(90, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(135, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(180, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(225, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(270, Vector3.up) * transform.forward,
            Quaternion.AngleAxis(315, Vector3.up) * transform.forward
        };

        foreach (Vector3 direction in angles) {
            Debug.DrawRay(transform.position, direction * 300, Color.blue);
        }

        Debug.DrawRay(transform.position, agent.destination, Color.red);

        Vector3 startPoint = transform.position + rayOffset;

        /*Vector3 playerDirection = (player.transform.position + rayOffset) - startPoint;
        RaycastHit hitPlayer;
        bool hasHit = Physics.Raycast(startPoint, playerDirection, out hitPlayer, maxShootingDistance) ;

        if (hasHit && hitPlayer.collider.gameObject.name == playerBody.name) {
            agent.isStopped = true;
            Debug.DrawRay(startPoint, playerDirection * hitPlayer.distance, Color.green);
        }
        else*/
        if (timer >= wanderTime || pathComplete() || agent.isStopped) {
            agent.isStopped = false;

            Random random = new Random();
            Vector3 direction;
            NavMeshHit outHit;

            if (random.Next(0, 10) < 3) {
                Debug.Log("rand");
                //30% chance of choosing random direction
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxWanderDistance;
                direction = randomDirection += transform.position;
            }
            else {
                Debug.Log("dir");
                //else, choose the distance with the longest open space
                float longestDistance = 0;
                int longestHit = 0;

                for (int i = 0; i < angles.Length; i++) {
                    RaycastHit rayHit;
                    bool hasHit = Physics.Raycast(transform.position, angles[i], out rayHit, maxShootingDistance);

                    if (!hasHit) { continue; }
                    else if (longestDistance < rayHit.distance) {
                        longestDistance = rayHit.distance;
                        longestHit = i;
                    }
                    
                }

                direction = angles[longestHit];
            }

            NavMesh.SamplePosition(direction, out outHit, maxWanderDistance, -1);
            agent.SetDestination(outHit.position);

            timer = 0;

            /*Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxWanderDistance;
            randomDirection += transform.position;
            NavMeshHit navHit;

            for (int i = 0; i < 10; i++) {
                NavMesh.SamplePosition(randomDirection, out navHit, maxWanderDistance, -1);
                agent.SetDestination(navHit.position);

                //Since the navmesh generates on the roof, reject new points that have too much height difference
                if (Mathf.Abs(navHit.position.y - transform.position.y) < HEIGHT_MIX) break;
            }


            timer = 0;


            //agent.SetDestination(player.transform.position);
            //Debug.DrawRay(startPoint, playerDirection * hitPlayer.distance, Color.yellow);*/
        }

    }

    protected bool pathComplete() {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance) {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
        }

        return false;
    }
}
