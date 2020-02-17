using System;
using System.Collections.Generic;
using System.Linq;
using ActionsIO;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SeedsSelectionMenu : MonoBehaviour
{
    public Animator animator;
    public List<TileActionsButton> seedBtns;

    public void Awake()
    {
        seedBtns = GetComponentsInChildren<TileActionsButton>().ToList();
    }

    public void Show(Transform _plantBtn)
    {
        foreach (TileActionsButton btn in seedBtns)
        {
            var action = btn.GetComponent<TileAction>();
            Debug.Log(action.vegetable + " veg ");
            VegetableData veg = VegetableManager.instance.Get(action.vegetable);

            btn.gameObject.SetActive(veg.unlocked);
            Debug.Log(btn.gameObject.name + " unlocked "+ veg.unlocked);
        }
        
        transform.position = _plantBtn.position - _plantBtn.up * 0.4f;
        transform.rotation = _plantBtn.rotation;
        animator.SetBool("show",true);
    }

    public void Hide()
    {
        animator.SetBool("show",false);
        foreach (TileActionsButton btn in seedBtns)
        {
            btn.gameObject.SetActive(false);
        }
    }
    
}