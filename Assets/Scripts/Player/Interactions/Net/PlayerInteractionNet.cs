using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Wildflare.Player.Combat;

namespace Wildflare.Player.Interactions.Net
{
    public class PlayerInteractionNet : NetworkBehaviour
    {
        [SyncVar(hook=nameof(HandleHealthChange))] int health = 3;
        public int health_gs {get {return health;} set{health = value;}}

        [SerializeField] Material green;
        [SerializeField] Material orange;
        [SerializeField] Material red;

        void HandleHealthChange(int oldHealth, int newHealth)
        {
            ChangeColor(newHealth);

            if(newHealth == 0)
            {
                CmdResetHealth();
                GetComponent<Grapple>().InstantStop();
                transform.position = new Vector3(0, 10, 0);
            }
        }

        void ChangeColor(int _newHealth)
        {
            if(_newHealth == 3) transform.GetChild(0).GetComponent<Renderer>().material = green;
            else if(_newHealth == 2) transform.GetChild(0).GetComponent<Renderer>().material = orange;
            else if(_newHealth == 1) transform.GetChild(0).GetComponent<Renderer>().material = red;
        }

        [Command] void CmdResetHealth() => health = 3;
    }
}

