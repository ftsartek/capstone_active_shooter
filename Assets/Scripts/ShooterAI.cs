using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using static UnityEngine.Physics;

public class ShooterAI : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject playerBody;

    [SerializeField]
    private Vector3 rayOffset = new Vector3(0, 30, 0);

    [SerializeField]
    private float maxShootingDistance = Mathf.Infinity;

    void FixedUpdate() {
        Vector3 startPoint = transform.position + rayOffset;
        Vector3 rayDirection = (player.transform.position + rayOffset) - startPoint;
        RaycastHit hit;
        bool hasHit = Physics.Raycast(startPoint, rayDirection, out hit, maxShootingDistance);

        if (hasHit && hit.collider.gameObject.name == playerBody.name) {
            agent.isStopped = true;
            Debug.DrawRay(startPoint, rayDirection * hit.distance, Color.green);
        }
        else {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            Debug.DrawRay(startPoint, rayDirection * hit.distance, Color.yellow);
        }

    }
}
