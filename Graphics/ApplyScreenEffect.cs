using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ApplyScreenEffect : MonoBehaviour
{

    public Shader effectShader;
    public float intensity = 1f;
    public float lerpSpeed = 1f;

    // private member variables
    private Material effectMat;
    private bool isLerping;
    private bool fadingIn = true;
    private bool active = true;

    private void Awake()
    {
        // Create a new material with the supplied shader.
        effectMat = new Material(effectShader);
    }

    // Update is called once per frame
    void Update()
    {

        if (isLerping && fadingIn)
        {
            if (intensity < 1f)
                intensity += Time.deltaTime * lerpSpeed;
            else
            {
                intensity = 1f;
                isLerping = false;
            }
        }
        else if (isLerping && !fadingIn)
        {
            if (intensity > 0f)
                intensity -= Time.deltaTime * lerpSpeed;
            else
            {
                intensity = 0f;
                isLerping = false;
                active = false;
            }
        }

        effectMat.SetFloat("_Intensity", intensity);
    }

    // OnRenderImage() is called when the camera has finished rendering.
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (active)
        {
            Graphics.Blit(src, dst, effectMat);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }

    public void ActivateDebuff(float dur)
    {
        Invoke("StopDebuff", dur);
        active = true;
        isLerping = true;
        fadingIn = true;
    }
    private void StopDebuff()
    {
        isLerping = true;
        fadingIn = false;
    }
}
