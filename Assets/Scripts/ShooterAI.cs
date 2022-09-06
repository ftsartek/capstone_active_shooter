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

enum State {
    Wandering,
    Shooting,
    Chasing,
    NewWanderGoal
}

public class ShooterAI : MonoBehaviour {
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private Vector3 rayOffset = new Vector3(0, 30, 0);
    [SerializeField] private GameObject marker;

    //Timers
    private static float wanderTime = 1000;
    private static float chaseTime = 50;

    //Goals
    private static int goalCount = 20;
    private static int goalOffset = 90;

    //Distances
    private static int chaseRadius = 2000;
    private static int goalRadius = 4000;
    private static float shotRadius = Mathf.Infinity;

    private Vector3[] goals;
    private float timer;
    private State state = State.Wandering;
    private GameObject chaseTarget;

    void OnEnable() {
        timer = wanderTime;

        //TODO: Replace markers with less hacky way of showing goals
        goals = new Vector3[goalCount];

        for (int i = 0; i < goals.Length; i++) {
            //choose a random place on the navmesh
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

        Vector3 playerDirection = (player.transform.position + rayOffset) - startPoint;
        RaycastHit playerHit;
        bool hasHit = Physics.Raycast(startPoint, playerDirection, out playerHit, shotRadius);

        if (state != State.Shooting && hasHit && playerHit.collider.gameObject.name == playerBody.name) {
            state = State.Shooting;
        }

        switch (state) {
            case State.Shooting:
                Debug.DrawRay(startPoint, playerDirection * playerHit.distance, Color.green);

                agent.isStopped = true;
                chaseTarget = player;

                if(!hasHit || playerHit.collider.gameObject.name != playerBody.name) {
                    Debug.Log("chase begun");

                    timer = 0;
                    state = State.Chasing;
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

                Random random = new Random();
                agent.SetDestination(goals[random.Next(0, goals.Length)]);
                break;

            case State.Wandering:
                agent.isStopped = false;

                if (timer >= wanderTime || pathComplete()) {
                    state = State.NewWanderGoal;
                }
                break;
        }

    }

    private bool pathComplete() {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= goalOffset) {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
        }

        return false;
    }
}
