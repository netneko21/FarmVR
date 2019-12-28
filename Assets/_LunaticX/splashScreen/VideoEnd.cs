using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoEnd : MonoBehaviour
{

    public UnityEngine.Video.VideoPlayer vp;
    void Start()
    {
        vp.loopPointReached += EndReached;

    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EndReached(vp);
        }

    }
    void EndReached(UnityEngine.Video.VideoPlayer _vp)
    {
        vp.enabled = false;
        SceneManager.LoadScene(1);
    }
}
