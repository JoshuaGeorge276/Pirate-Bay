using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTransitions : MonoBehaviour {

    public Texture directionTexture;
    public Texture transitionTexture;

    private Material mat;

    private void Start()
    {
        mat = new Material(Shader.Find("Custom/BattleTransition"));
        mat.SetTexture("_DirTex", directionTexture);
        mat.SetTexture("_TransitionTexture", transitionTexture);

        StartCoroutine(Pulse(3, 0.5f));
    }

    IEnumerator Pulse(float speed, float fadeInTime)
    {
        StartCoroutine(FadeToBlack(speed));
        yield return new WaitForSeconds(fadeInTime);
        StartCoroutine(FadeFromBlack(speed));
    }

    IEnumerator FadeToBlack(float speed)
    {
        float fade = 0;
        while(fade < 1) {
            fade += Time.deltaTime * speed;
            mat.SetFloat("_Fade", fade);
            yield return null;
        }
    }

    IEnumerator FadeFromBlack(float speed)
    {
        float fade = 1;
        while (fade > 0)
        {
            fade -= Time.deltaTime * speed;
            mat.SetFloat("_Fade", fade);
            yield return null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat)
        {
            Graphics.Blit(source, null, mat);
        }
    }
}
