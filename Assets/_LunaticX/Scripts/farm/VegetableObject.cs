
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VegetableObject : MonoBehaviour
{
    public Vector3 scale;
    public float time, timer, fadeTimer;
    public bool showing, fading;
    private Renderer r;
    MaterialPropertyBlock props;

    void Awake()
    {
        r = GetComponent<Renderer>();
        props = new MaterialPropertyBlock();
    }

    public void UpdateMe()
    {
        time += Time.deltaTime;
        transform.localScale = Vector3.Lerp(scale, Vector3.one, time / timer);
    }

    private Coroutine currentCR;

    private IEnumerator ShowCR()
    {
        fadeTimer = 0;

        while (fadeTimer < 1)
        {
            fadeTimer += Time.deltaTime;
            props.SetFloat("_alpha", fadeTimer);
            r.SetPropertyBlock(props);
            yield return 0;
        }

        props.SetFloat("_alpha", 1);
        r.SetPropertyBlock(props);
    }

    private IEnumerator HideCR()
    {
        fadeTimer = 1;

        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            props.SetFloat("_alpha", fadeTimer);
            r.SetPropertyBlock(props);
            yield return 0;
        }

        Destroy(gameObject);
    }

    public void Show(Vector3 _scale, float _timer)
    {
        fadeTimer = 0;
        scale = _scale;
        time = 0;
        timer = _timer;
        transform.localScale = scale;
        transform.eulerAngles = new Vector3(0, Random.Range(0, 359));
        if(currentCR!=null){StopCoroutine(currentCR);}
        currentCR = StartCoroutine(ShowCR());
    }

    public void Hide()
    {
        if(currentCR!=null){StopCoroutine(currentCR);}
        currentCR = StartCoroutine(HideCR());
    }
}