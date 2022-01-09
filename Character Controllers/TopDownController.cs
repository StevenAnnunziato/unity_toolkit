using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{

    [Header("Player Attributes")]
    public float moveSpeed = 2f;

    [Header("Object References")]
    private CharacterController cc;
    
    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // get input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // normalize movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveY);
        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        // apply movement
        movement *= moveSpeed;
        cc.SimpleMove(movement);

    }
}
