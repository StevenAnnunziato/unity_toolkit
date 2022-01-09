using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextFlyby : MonoBehaviour
{

    [Header("Set Curve Here")]
    public AnimationCurve textFlybyCurve; // represents x position over time on the interval [-endPos, endPos]

    [Header("Custom Colors")]
    public Color speedColor;
    public Color damageColor;
    public Color healthColor = Color.green;
    public Color waveColor;

    // private member variables
    public static TextFlyby sharedInstance;
    private GameObject textObject;
    private bool isAnimating;
    private RectTransform flybyTextTransform;
    private Shadow shadow;
    private Text flybyText;
    private float currentTime;
    private float endPos;
    private float startTime;

    private void Awake()
    {
        // TextFlyby is a singleton - destroy any other ones
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        textObject = GameObject.FindGameObjectWithTag("PowerupText");

        flybyTextTransform = textObject.GetComponent<RectTransform>();
        shadow = textObject.GetComponent<Shadow>();
        flybyText = textObject.GetComponent<Text>();

        endPos = textFlybyCurve.keys[0].value;
        startTime = textFlybyCurve.keys[0].time;

        Vector2 currentPos = flybyTextTransform.anchoredPosition;
        currentPos.x = textFlybyCurve.Evaluate(startTime);
        flybyTextTransform.anchoredPosition = currentPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            FlashText();
        }
    }

    public void FlashText()
    {

        // move text across the screen
        Vector2 currentPos = flybyTextTransform.anchoredPosition;
        currentPos.x = textFlybyCurve.Evaluate(currentTime);
        flybyTextTransform.anchoredPosition = currentPos;

        currentTime += Time.deltaTime;

        if (currentTime >= -startTime)
        {
            isAnimating = false;
        }
    }

    public void AnimateText(string text, Color color)
    {
        // initialize values
        isAnimating = true;
        currentTime = startTime;

        // set appropriate text and shadow color
        shadow.effectColor = color;
        flybyText.text = text;

        // move text off the screen to the left
        Vector2 currentPos = flybyTextTransform.anchoredPosition;
        currentPos.x = -endPos;
        flybyTextTransform.anchoredPosition = currentPos;
    }

    public void AnimatePowerupText(int type)
    {
        if (type == 0) // speed
        {
            AnimateText("SPEED UP!", speedColor);
        }
        else if (type == 1) // damage
        {
            AnimateText("DAMAGE UP!", damageColor);
        }
        else if (type == 2) // health
        {
            AnimateText("RECOVERED HP!", healthColor);
        }
    }

    public void AnimateWave(int num)
    {
        AnimateText("WAVE " + num, waveColor);
    }
    public void AnimateComplete()
    {
        AnimateText("WAVE COMPLETE!", waveColor);
    }

}
