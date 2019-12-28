using System;
using UnityEngine;

namespace ActionsIO
{
    public class ActionIO : MonoBehaviour
    {
        public ActionMode mode = ActionMode.Run;
        public InteractiveObject io;
        public void Validate()
        {
            if (io != null) return;
            io = GetComponent<InteractiveObject>();
            if(!io){Debug.LogError("no target io object on action " + this);}
            io.AddAction(this);
        }
        
        public virtual void Run()
        {
            
        }
        
        public virtual void Stop()
        {
            
        }

        public virtual void Remove()
        {
            io.RemoveAction(this);
            Destroy(this);
        }

    }
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentIdentifierAttribute : Attribute
    {
        public ItemActions action {get; set;}
    }
}
