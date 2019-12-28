
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileActionsButton : MonoBehaviour
{
    public Image bg, icon;
    public void ActivateButton(bool _isActive)
    {
        if (_isActive)
        {
            bg.color = Color.white;
            icon.color = Color.white;
            GetComponent<InteractiveObject>().Enable();
        }
        else
        {

            bg.color = Color.gray;
            icon.color = Color.gray;
            GetComponent<InteractiveObject>().Disable();
        }
    }

}
