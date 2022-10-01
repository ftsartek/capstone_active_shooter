using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Physics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Random = System.Random;
using UnityEngine.UIElements;
using System;

public enum State {
    Wandering,
    Shooting,
    Chasing,
    NewWanderGoal,
    Exiting
}

public class ShooterAI : MonoBehaviour {
    private NavMeshAgent agent;
    private GameObject player;
    private GameObject playerBody;
    private GameObject marker;
    private GameObject bullet;

    //Settings
    public bool hasMarkers = true;
    public bool constantY = false;

    //Offsets
    private static Vector3 rayOffset = new Vector3(0, 0.5f, 0);
    private static Vector3 bulletOffset = new Vector3(0, 0.7f, 0);

    //Timers
    private static float wanderTime = 1000;
    private static float chaseTime = 50;
    public float exitTime = 5000;
    private static float bulletTime = 1;

    //Goals
    private static int goalCount = 20;
    private static float goalOffset = 2;
    private static float goalVOffset = 0.1f;

    //Distances
    public static int chaseRadius = 50;
    public static int goalRadius = 100;
    private static float shotRadius = Mathf.Infinity;

    private static float bulletForce = 1000;

    [HideInInspector] public Vector3[] goals;
    [HideInInspector] public float timer;
    [HideInInspector] public float exitTimer;
    [HideInInspector] public State state = State.Wandering;
    [HideInInspector] public GameObject chaseTarget;

    private Random random = new Random();

    private void Awake() {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerBody = player.transform.Find("PlayerBody").gameObject;
        marker = (GameObject) Resources.Load("Marker");
        bullet = (GameObject) Resources.Load("Bullet");
    }

    void OnEnable() {
        timer = wanderTime;
        exitTimer = 0;

        //TODO: Replace markers with less hacky way of showing goals
        goals = new Vector3[goalCount];

        for (int i = 0; i < goals.Length; i++) {
            //choose a random place on the navmesh
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * goalRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;

            NavMesh.SamplePosition(randomDirection, out navHit, goalRadius, 1);

            goals[i] = navHit.position;

            if (constantY) {
                goals[i].y = player.transform.position.y + goalVOffset;
            }

            if (hasMarkers) {
                Instantiate(marker, goals[i], Quaternion.identity);
            }
        }
    }

    void FixedUpdate() {
        timer += Time.deltaTime;
        exitTimer += Time.deltaTime;

        Vector3 startPoint = transform.position + rayOffset;
        Vector3 playerDirection = (player.transform.position + rayOffset) - startPoint;

        RaycastHit playerHit;
        bool hasHit = Physics.Raycast(startPoint, playerDirection, out playerHit, shotRadius);
        GameObject closestTarget = getTarget(hasHit, playerHit);

        if (state != State.Exiting && exitTimer > exitTime) {
            state = State.Exiting;
        }
        else if (state != State.Shooting && closestTarget != null) {
            state = State.Shooting;
            chaseTarget = player;
        }

        switch (state) {
            case State.Shooting:
                Debug.DrawRay(startPoint, playerDirection * playerHit.distance, Color.green);

                agent.isStopped = true;
                chaseTarget = player;

                if (!closestTarget) {
                    Debug.Log("chase begun");

                    timer = 0;
                    state = State.Chasing;
                }
                else if (closestTarget != chaseTarget) {
                    //focus on the closest target, not follow the first to the ends of the earth
                    //i.e. if one wanders out of sight but another is closer, target that instead
                    chaseTarget = closestTarget;
                }

                break;

            case State.Chasing:
                Debug.DrawRay(startPoint, playerDirection * playerHit.distance, Color.yellow);

                agent.isStopped = false;
                agent.SetDestination(chaseTarget.transform.position);

                if (Vector3.Distance(agent.destination, agent.transform.position) > chaseRadius
                        || timer >= chaseTime) {
                    state = State.NewWanderGoal;
                }

                break;

            case State.NewWanderGoal:
                //this is its own state as it can be troggered by two actions
                Debug.DrawRay(startPoint, playerDirection * playerHit.distance, Color.red);
                Debug.Log("new random target chosen");

                agent.isStopped = false;
                chaseTarget = null;

                state = State.Wandering;
                timer = 0;

                agent.SetDestination(generateNextGoal());
                break;

            case State.Wandering:
                agent.isStopped = false;

                if (timer >= wanderTime || pathComplete()) {
                    state = State.NewWanderGoal;
                }
                break;

            case State.Exiting:
                Debug.Log("exiting");

                agent.SetDestination(GameObject.Find("MainExit").transform.position);
                break;
        }

    }

    private void Update() {
        if (state == State.Shooting && timer > bulletTime) {
            Vector3 startPoint = transform.position + bulletOffset;
            Vector3 playerDirection = (player.transform.position + bulletOffset) - startPoint;

            timer = 0;
            GameObject bulletInstance = Instantiate(bullet, startPoint, transform.rotation);
            bulletInstance.GetComponent<Rigidbody>().AddForce(playerDirection * bulletForce);
        }
    }

    private Vector3 generateNextGoal() {
        //generate a goal that is random but not too close to the current one
        //currently furthest out of n iterations but could change

        Vector3 furthest = goals[0];
        float furthestDistance = 0;

        for (int i = 0; i < 2; i++) {
            Vector3 candidate = goals[random.Next(0, goals.Length)];
            float candidateDistance = Vector3.Distance(candidate, transform.position);

            if (candidateDistance > furthestDistance) {
                furthest = candidate;
                furthestDistance = candidateDistance;
            }
        }

        return furthest;
    }

    private GameObject getTarget(bool enable, RaycastHit hit) {
        if (!enable) return null;

        if (hit.collider.gameObject.name == playerBody.name) {
            return playerBody.transform.parent.gameObject;
        }
        else if (hit.collider.gameObject.tag == "InnocentAI") {
            return hit.collider.gameObject;
        }
        return null;
    }

    private bool pathComplete() {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= goalOffset) {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
        }

        return false;
    }
}
