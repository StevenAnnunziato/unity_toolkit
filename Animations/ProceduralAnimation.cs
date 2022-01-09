using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimation : MonoBehaviour
{
    [Header("General Settings")]
    public float playbackSpeed = 1f;
    public AnimationCurve timeCurve;

    [Header("Position")]
    public Vector3 startingPos;
    public Vector3 endPos;
    public AnimationCurve movementCurveX;
    public AnimationCurve movementCurveY;
    public AnimationCurve movementCurveZ;

    [Header("Rotation")]
    public Vector3 startingRot;
    public Vector3 endRot;
    public AnimationCurve rotationCurveX;
    public AnimationCurve rotationCurveY;
    public AnimationCurve rotationCurveZ;

    // private member variables
    private bool isAnimating;
    private float animProgress;
    private bool backwards;

    // Start is called before the first frame update
    void Start()
    {

        // put the UI in its starting position
        transform.localPosition = startingPos;
        transform.localRotation = Quaternion.Euler(startingRot);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimateFromStart();
        }

        // animate
        if (isAnimating)
        {

            // calculate and assign position
            Vector3 newPos;
            newPos.x = Mathf.LerpUnclamped(startingPos.x, endPos.x, movementCurveX.Evaluate(animProgress));
            newPos.y = Mathf.LerpUnclamped(startingPos.y, endPos.y, movementCurveY.Evaluate(animProgress));
            newPos.z = Mathf.LerpUnclamped(startingPos.z, endPos.z, movementCurveZ.Evaluate(animProgress));
            transform.localPosition = newPos;

            // calculate and assign rotation
            Vector3 newRot;
            newRot.x = Mathf.LerpUnclamped(startingRot.x, endRot.x, rotationCurveX.Evaluate(animProgress));
            newRot.y = Mathf.LerpUnclamped(startingRot.y, endRot.y, rotationCurveY.Evaluate(animProgress));
            newRot.z = Mathf.LerpUnclamped(startingRot.z, endRot.z, rotationCurveZ.Evaluate(animProgress));
            transform.localRotation = Quaternion.Euler(newRot);

            // assign time
            Time.timeScale = Mathf.LerpUnclamped(0f, 1f, timeCurve.Evaluate(animProgress));

            // continue animation
            if (backwards)
                animProgress -= Time.deltaTime * playbackSpeed;
            else
                animProgress += Time.deltaTime * playbackSpeed;

            // stop animation
            if (animProgress > 1f || animProgress < 0f)
                isAnimating = false;
        }

    }

    public bool Animate()
    {
        // don't do this if already animating
        if (isAnimating)
            return false;

        // assign direction
        if (animProgress > 1f)
            backwards = true;
        else if (animProgress < 0)
            backwards = false;

        // reset
        isAnimating = true;
        if (backwards)
            animProgress = 1f;
        else
            animProgress = 0f;

        // put the UI in its starting position
        transform.localPosition = startingPos;

        return true;
    }

    public bool AnimateFromStart()
    {
        // don't do this if already animating
        if (isAnimating)
            return false;

        // reset
        isAnimating = true;
        if (backwards)
            animProgress = 1f;
        else
            animProgress = 0f;

        // put the UI in its starting position
        transform.localPosition = startingPos;
        transform.localRotation = Quaternion.Euler(startingRot);

        return true;
    }

    // for UI buttons
    public void StartAnimation()
    {
        Animate();
    }

    public float getProgress() { return animProgress; }

}
