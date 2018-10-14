using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTransitions : MonoBehaviour {

    public Color colour;
    public Texture directionTexture;
    public Texture transitionTexture;
    public Texture transitionToGameTexture;

    [Range(0, 5)]
    public int flashCount = 2;
    public float flashSpeed = 3.0f;
    public float fadeDuration = 0.2f;
    public float transitionSpeed = 1.0f;

    private Material mat;

    private void Start()
    {
        mat = new Material(Shader.Find("Custom/BattleTransition"));
        mat.SetTexture("_DirTex", directionTexture);
        mat.SetTexture("_TransitionTex", transitionTexture);
        mat.SetColor("_Color", colour);
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
            fade -= Time.smoothDeltaTime * speed;
            mat.SetFloat("_Fade", fade);
            yield return null;
        }
    }

    IEnumerator TransitionToBlack(float speed, bool distort)
    {
        mat.SetTexture("_TransitionTex", transitionTexture);
        mat.SetFloat("_Fade", 0);
        mat.SetFloat("_Distort", (distort) ? 1 : 0);

        float cutoff = 0;

        while(cutoff < 1)
        {
            cutoff += Time.smoothDeltaTime * speed;
            mat.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

    }

    IEnumerator TransitionToGame(float speed)
    {
        mat.SetTexture("_TransitionTex", transitionToGameTexture);
        mat.SetFloat("_Fade", 0);

        float cutoff = 1;

        while (cutoff > 0)
        {
            cutoff -= Time.smoothDeltaTime * speed;
            mat.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

    }

    IEnumerator Play(int flashCount, float fadeSpeed = 1f, float fadeDuration = 0.5f, float transitionSpeed = 1f)
    {
        for(int i = 0; i < flashCount; i++)
        {
            StartCoroutine(Pulse(fadeSpeed, fadeDuration));
            yield return new WaitForSeconds(fadeDuration + 0.5f);
        }

        StartCoroutine(TransitionToBlack(transitionSpeed, false));

        yield return new WaitForSeconds(3.0f);

        StartCoroutine(TransitionToGame(1.5f));
    }

    [ContextMenu("Play Battle Transition")]
    private void Play()
    {
        if (mat == null)
        {
            Debug.LogError("Must be in play mode!");
            return;
        }

        StartCoroutine(Play(flashCount, flashSpeed, fadeDuration, transitionSpeed));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat)
        {
            Graphics.Blit(source, null, mat);
        }
    }
}
