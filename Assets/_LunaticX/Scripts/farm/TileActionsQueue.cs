
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
        
public class TileActionsQueue : MonoBehaviour
{
        [SerializeField]
        public  Animator animator;
        
        [SerializeField]
        public  Transform girlT;
        
        [SerializeField]
        public  List<TileActionQ> actions = new List<TileActionQ>();
        public  void AddAction(Tile _tile,TileMenu.ActionType _type,VegType _vegetable = VegType.empty)
        {
               actions.Add(new TileActionQ(_tile,_type,_vegetable));
        } 
        
        public  void GetNextAction()
        {
                CurrentAction = actions.PopAt(0);
        }
        
        public  void ClearQueue()
        {
                actions.Clear();
        }
        public static TileActionsQueue instance;

        public TileActionQ _currentAction;
        public TileActionQ CurrentAction {
                get { return _currentAction; } set{_currentAction = value;}  }

        void Awake()
        {
                instance = this;
                CurrentAction = null;
                animator = GetComponent<Animator>();
                girlT = transform;
        }

        void Update()
        {
                if (CurrentAction == null && actions.Count > 0)
                {
                        GetNextAction();
                        DoAction();
                }
        }
        
        public void DoAction()
        {
                if (CurrentAction == null){ return;}
                //MoveToTile();
                EndAction();
                //PlayAnimation();
        }

        public  void MoveToTile()
        {
                Vector3 pos = CurrentAction.tile.transform.position - Vector3.forward*0.8f;
                pos.y = 0.1f;
                girlT.position = pos;
        }

        public  void PlayAnimation()
        {
                animator.SetBool("idle",false);
                switch (CurrentAction.type)
                {
                        
                        case TileMenu.ActionType.Dig:
                                animator.SetTrigger("Dig");
                                break;
                        case TileMenu.ActionType.Clear:
                                animator.SetTrigger("Clear");
                                break;
                        case TileMenu.ActionType.Harvest:
                                animator.SetTrigger("Harvest");
                                break;
                        case TileMenu.ActionType.Plant:
                                animator.SetTrigger("Plant");
                                break;
                        case TileMenu.ActionType.Water:
                                animator.SetTrigger("Water");
                                break;
                        
                }
        }
        
        public  void EndAction()
        {
                switch (CurrentAction.type)
                {
                        
                        case TileMenu.ActionType.Dig:
                                CurrentAction.tile.DigExecute();
                                break;
                        case TileMenu.ActionType.Clear:
                                CurrentAction.tile.ClearExecute();;
                                break;
                        case TileMenu.ActionType.Harvest:
                                CurrentAction.tile.HarvestExecute();
                                break;
                        case TileMenu.ActionType.Plant:
                                CurrentAction.tile.PlantSeedExecute(CurrentAction.vegetable);
                                break;
                        case TileMenu.ActionType.Water:
                                CurrentAction.tile.WaterExecute();
                                break;
                        
                }
                
             //   actions.Dequeue();
                CurrentAction = null;
        }

        public  void StopAnimation()
        {
                animator.SetBool("idle",true);
        }
        
        //animation event
        public void ActionAnimationFinished()
        {
                if (CurrentAction == null) return;
                StopAnimation();
                EndAction();
        }
}



public class TileActionQ
{
        public Tile tile;
        public TileMenu.ActionType type;
        public VegType vegetable;

        public TileActionQ(Tile _tile,TileMenu.ActionType _type,VegType _vegetable)
        {
                tile = _tile;
                type = _type;
                vegetable = _vegetable;
        }
}

public static class ListExtensionMethods
{
        public static T PopAt<T>(this List<T> list, int index)
        {
                var r = list[index];
                list.RemoveAt(index);
                return r;
        }

        public static T PopFirst<T>(this List<T> list, Predicate<T> predicate)
        {
                var index = list.FindIndex(predicate);
                var r = list[index];
                list.RemoveAt(index);
                return r;
        }

        public static T PopFirstOrDefault<T>(this List<T> list, Predicate<T> predicate) where T : class
        {
                var index = list.FindIndex(predicate);
                if (index > -1)
                {
                        var r = list[index];
                        list.RemoveAt(index);
                        return r;
                }
                return null;
        }
}
