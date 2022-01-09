using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateMenus : MonoBehaviour
{

    [Header("Animation Settings")]
    public Vector2 startingPos;
    public Vector2 endPos;
    public AnimationCurve movementCurve;
    public float playbackSpeed = 1f;
    public bool animateOnStart;

    // private member variables
    private RectTransform uiToAnimate;
    private bool isAnimating;
    private float animProgress;
    private bool backwards;

    // Start is called before the first frame update
    void Start()
    {

        uiToAnimate = GetComponent<RectTransform>();

        // put the UI in its starting position
        uiToAnimate.anchoredPosition = startingPos;

        if (animateOnStart)
            Animate();
    }

    // Update is called once per frame
    void Update()
    {

        // animate
        if (isAnimating)
        {
            // calculate and assign position
            uiToAnimate.anchoredPosition = Vector2.LerpUnclamped(startingPos, endPos, movementCurve.Evaluate(animProgress));

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
        uiToAnimate.anchoredPosition = startingPos;

        return true;
    }

    public bool AnimateForwards()
    {
        // don't do this if already animating
        if (isAnimating)
            return false;

        // reset
        isAnimating = true;
        backwards = false;
        animProgress = 0f;

        // put the UI in its starting position
        uiToAnimate.anchoredPosition = startingPos;

        return true;
    }
    public bool AnimateBackwards()
    {
        // don't do this if already animating
        if (isAnimating)
            return false;

        // reset
        isAnimating = true;
        backwards = true;
        animProgress = 1f;

        // put the UI in its starting position
        uiToAnimate.anchoredPosition = startingPos;

        return true;
    }

    // for UI buttons
    public void StartAnimation()
    {
        Animate();
    }

    public float getProgress() { return animProgress; }

    public void SetText(string text)
    {
        GetComponent<Text>().text = text;
    }

}
