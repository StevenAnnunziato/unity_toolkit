using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    private Text fpsText;
    private float deltaTime;

    public Color goodColor;
    public Color medColor;
    public Color badColor;

    public float maxCooldown = 0.1f;
    private float curCooldown = 0f;

    private void Start()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        curCooldown -= Time.deltaTime;
        if (curCooldown <= 0)
        {
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
            curCooldown = maxCooldown;

            // set color
            if (fps >= 144f)
                fpsText.color = goodColor;
            else if (fps < 144f && fps >= 60f)
                fpsText.color = medColor;
            else
                fpsText.color = badColor;
        }
    }
}