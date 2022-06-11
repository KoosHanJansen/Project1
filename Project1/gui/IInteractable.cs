using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Project1
{
    interface IInteractable
    {
        public event EventHandler Interact;

        public void OnInteract()
        {
            
        }
    }
}
