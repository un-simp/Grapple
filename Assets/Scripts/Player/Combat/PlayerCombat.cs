using UnityEngine;
using System;
using System.Collections;
using Wildflare.Player.Interfaces;
using Mirror;

namespace Wildflare.Player.Combat
{
    public class PlayerCombat : NetworkBehaviour
    {
        public GameObject[] weapons;

        private PlayerMovement movement;

        public IWeapon currentActiveWeapon;

        private weaponType currentWeaponType;

        enum weaponType {
            Grapple, Glock, Awp, Shotgun
        }

        private bool inTransition = false;

        public void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            if(!hasAuthority) return;
            WeaponSwitcher();
        }

        void WeaponSwitcher() {
            if (inTransition) return;
            if(Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponType != weaponType.Grapple)
            {
                SwitchWeapon((int)weaponType.Grapple);
                currentWeaponType = weaponType.Grapple;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponType != weaponType.Glock)
            {
                SwitchWeapon((int)weaponType.Glock);
                currentWeaponType = weaponType.Glock;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                //SwitchWeapon((int)weaponType.Deagle);
            }
        }

        void SwitchWeapon(int _weaponIndex) {
            StartCoroutine(TransitionDelay());
            for(int i = 0; i < weapons.Length; i++)
            {
                bool isHoldable = weapons[i].TryGetComponent(out IHoldable __activeWeaponSelect);
                if(i == _weaponIndex)
                {
                    weapons[i].SetActive(true);
                    __activeWeaponSelect.AnimateIn();
                    CmdChangeWeaponState(i, true);
                    if(isHoldable)
                    {
                        __activeWeaponSelect.OnSelect();
                        CmdChangeWeaponSelection(i, true);

                        if((__activeWeaponSelect as MonoBehaviour) is IWeapon)
                            currentActiveWeapon = (__activeWeaponSelect as IWeapon);
                        else
                            currentActiveWeapon = null;
                    }
                    continue;
                }
                if(isHoldable)
                {
                    __activeWeaponSelect.OnDeselect();
                    CmdChangeWeaponSelection(i, false);
                }
                weapons[i].SetActive(false);
                CmdChangeWeaponState(i, false);
            }
        }

        IEnumerator TransitionDelay() {
            inTransition = true;
            yield return new WaitForSeconds(0.55f);
            inTransition = false;
        }

        [Command] void CmdChangeWeaponState(int _index, bool _state) => RpcChangeWeaponState(_index, _state);
        [ClientRpc(includeOwner=false)] void RpcChangeWeaponState(int _index, bool _state) => weapons[_index].SetActive(_state);

        [Command] void CmdChangeWeaponSelection(int _index, bool _isSelected) => RpcChangeWeaponSelection(_index, _isSelected);
        [ClientRpc(includeOwner=false)] void RpcChangeWeaponSelection(int _index, bool _isSelected){
            if(weapons[_index].TryGetComponent(out IHoldable __activeWeaponSelect))
            {
                if(_isSelected)
                    __activeWeaponSelect.OnSelect();
                else
                    __activeWeaponSelect.OnDeselect();
            }
        }
    }
}

