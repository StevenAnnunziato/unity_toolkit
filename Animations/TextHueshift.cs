using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHueshift : MonoBehaviour
{

    public Color startingColor = Color.red;
    public float cycleSpeed = 0.05f;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startHSV;
        Color.RGBToHSV(startingColor, out startHSV.x, out startHSV.y, out startHSV.z);
        Color myColor = Color.HSVToRGB(Mathf.Repeat(startHSV.x + Time.time * cycleSpeed, 1f), startHSV.y, startHSV.z);
        text.color = myColor;
    }
}
