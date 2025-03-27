using System;
using UnityEngine;
using System.Collections;
using CommandState = GameManager.CommandState;
using Unity.VisualScripting;
public class YariponStateMachine : MonoBehaviour
{
    private GameManager gmSingleton;

    public Vector3 forceMovement;
    public Vector3 stoppingForceMovement;

    private Animator animator;
    private Rigidbody rb;

    private System.Random random;

    private bool triggerSetWalk;
    private bool moving;

    private CommandState previous;


    public GameObject ModelSpear; // the one to hide aka the models
    public GameObject Spear; //the one that we will throw aka physical object
    public Vector3 Offset;
    public Vector3 Rotation;
    public float strengthThrow;
    public Vector3 offsetStrength;
    void Start()
    {
        gmSingleton = GameManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();

        byte[] seedArray = new byte[4];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(seedArray);
        }
        int seed = BitConverter.ToInt32(seedArray, 0);
        random = new System.Random(seed);
        previous = CommandState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (previous != gmSingleton.commandState){ 
        switch (gmSingleton.commandState)
            {
                case CommandState.IDLE:
                    moving = false;
                    break;
                case CommandState.PATA:
                    moving = true;
                    break;
                case CommandState.PON:
                    animator.SetTrigger(Animator.StringToHash("Attack"));
                    break;
            }
            previous = gmSingleton.commandState;
        }
        else
        {
            previous = gmSingleton.commandState;
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.AddForce(forceMovement, ForceMode.Force);
            if (gmSingleton.feverLevel < 8 && !triggerSetWalk)
            {
                walkingnormalTrigger();
            }
            else if (gmSingleton.feverLevel >= 8 && !triggerSetWalk)
            {
                walkingfeverTrigger();
            }
        } else
        {
            triggerSetWalk = false;
        }
    }

    public void pataTrigger()
    {
        animator.SetTrigger(Animator.StringToHash("Pata"));
    }

    public void ponTrigger()
    {
        animator.SetTrigger(Animator.StringToHash("Pon"));
    }

    public void walkingnormalTrigger()
    {
        animator.SetTrigger(Animator.StringToHash("Walk"));
        triggerSetWalk = true;
    }

    public void walkingfeverTrigger()
    {
        animator.SetTrigger(Animator.StringToHash("WalkFever"));
        triggerSetWalk = true;
    } 

    public void ThrowSpear()
    {
        StartCoroutine(HideWeaponandUnhide(0.6f));
        // Get the current position of the GameObject and apply an offset
        Vector3 location = transform.position + Offset;

        // Instantiate the Spear GameObject at the computed location
        GameObject newSpear = Instantiate(Spear, location, Quaternion.identity);

        // Set the rotation of the new spear based on your desired orientation
        newSpear.transform.rotation = Quaternion.Euler(Rotation);

        Rigidbody rb = newSpear.GetComponent<Rigidbody>();

        rb.AddForce((newSpear.transform.forward+offsetStrength) * strengthThrow, ForceMode.Impulse);
    }
    private IEnumerator DelayedTrigger(string triggerName)
    {
        // Generate a random delay between 0 and 0.02 ms
        float delay = (float)(random.NextDouble() * 0.02); // Random.NextDouble gives 0.0 to 1.0
        yield return new WaitForSeconds(delay);

        // Set the trigger
        animator.SetTrigger(Animator.StringToHash(triggerName));
    }

    private IEnumerator DestroyTigger(string triggerName, float time)
    {
        yield return new WaitForSeconds(time);
    }

    private IEnumerator HideWeaponandUnhide(float time)
    {
        ModelSpear.GetComponent<SkinnedMeshRenderer>().enabled = false;
        yield return new WaitForSeconds(time);
        ModelSpear.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

}
