using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimation : MonoBehaviour
{
    [Header("Hover Properties")]
    public bool willHover = true;
    public float floatRange = 1f;
    public float period = 1f;
    public Vector3 hoverAxes = Vector3.up;
    public bool bounce = false;
    private float offset;

    // TODO: convert to vector3 rotation
    [Header("Rotation Properties")]
    public Vector3 rotation;
    public float rotationSpeed = 5f;

    [Header("Translation Properties")]
    public Vector3 translation;
    public float translateSpeed = 0f;

    // private member variables
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        offset = Random.Range(1f, 3f);
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        // hover
        if (willHover)
        {
            // calculate position in hover cycle
            float movement = Mathf.Sin(Time.timeSinceLevelLoad / period + offset) * floatRange;
            Vector3 valueToAdd = new Vector3(hoverAxes.x * movement, hoverAxes.y * movement, hoverAxes.z * movement);



            // finalize position
            if (!bounce)
                transform.localPosition += valueToAdd * Time.deltaTime;
            else // BOUNCE
                transform.localPosition = new Vector3(transform.position.x + valueToAdd.x, initialPos.y + Mathf.Abs(valueToAdd.y), transform.position.z + valueToAdd.z);
        }

        // rotate
        if (rotation != Vector3.zero)
            transform.Rotate(rotation * Time.deltaTime * rotationSpeed);

        // translate
        if (translation != Vector3.zero)
            transform.Translate(translation * Time.deltaTime * translateSpeed);
    }
}
