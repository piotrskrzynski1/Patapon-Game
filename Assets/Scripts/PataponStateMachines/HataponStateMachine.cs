using UnityEngine;
using CommandState = GameManager.CommandState;
public class HataponStateMachine : MonoBehaviour
{
    private GameManager gmSingleton;

    public Vector3 forceMovement;
    public Vector3 stoppingForceMovement;

    private Animator animator;
    private Rigidbody rb;

    private bool moving;
    void Start()
    {
        gmSingleton = GameManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gmSingleton.commandState)
        {
            case CommandState.IDLE:
                moving = false;
                animator.SetBool(Animator.StringToHash("Marching"), false);
                break;
            case CommandState.PATA:
                moving = true;
                animator.SetBool(Animator.StringToHash("Marching"), true);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.AddForce(forceMovement, ForceMode.Force);
        }
    }

}
