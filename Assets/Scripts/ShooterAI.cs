using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Physics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Random = System.Random;
using UnityEngine.UIElements;

public class ShooterAI : MonoBehaviour
{    public float wanderTime = 6;
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

    [SerializeField]
    private GameObject marker;

    public int goalCount = 20;
    public int goalRadius = 4000;

    private Vector3[] goals;

    void OnEnable() {
        timer = wanderTime;

        goals = new Vector3[goalCount];

        for (int i = 0; i < goals.Length; i++) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * goalRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;

            NavMesh.SamplePosition(randomDirection, out navHit, goalRadius, 1);

            goals[i] = navHit.position;
            goals[i].y = player.transform.position.y + 20;

            Instantiate(marker, goals[i], Quaternion.identity);
        }
    }
    
    void FixedUpdate() {
        timer += Time.deltaTime;

        Vector3 startPoint = transform.position + rayOffset;

        //TODO: Stop when player approaches
        //TODO: Choose another position if it stays too long in one place (possibly even strike bad goal off list?)
        /*Vector3 playerDirection = (player.transform.position + rayOffset) - startPoint;
        RaycastHit hitPlayer;
        bool hasHit = Physics.Raycast(startPoint, playerDirection, out hitPlayer, maxShootingDistance) ;

        if (hasHit && hitPlayer.collider.gameObject.name == playerBody.name) {
            agent.isStopped = true;
            Debug.DrawRay(startPoint, playerDirection * hitPlayer.distance, Color.green);
        }
        else*/ if (timer >= wanderTime || pathComplete() || agent.isStopped) {
            agent.isStopped = false;
            timer = 0;

            Random random = new Random();
            agent.SetDestination(goals[random.Next(0, goals.Length)]);
            
        }

    }

    protected bool pathComplete() {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance) {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
        }

        return false;
    }
}
