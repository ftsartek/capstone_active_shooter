using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{

    public AiStateMachine stateMachine;
    public AiStateId initialState;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.ChangeState(initialState); 
    }

    // Update is called once per frame
    void Update()
    {
      stateMachine.Update();
    }
}
