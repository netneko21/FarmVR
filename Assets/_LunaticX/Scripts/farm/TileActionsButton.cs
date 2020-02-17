
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileActionsButton : MonoBehaviour
{
    public Image bg, icon;
    public void ActivateButton(bool _isActive)
    {
        gameObject.SetActive(_isActive);
        if (_isActive)
        {
            //GetComponent<InteractiveObject>().Enable();
        }
        else
        {
            //GetComponent<InteractiveObject>().Disable();
        }
    }

}
