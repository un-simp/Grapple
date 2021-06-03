using System.Collections;
using Mirror;
using UnityEngine;
using Wildflare.Player.Interfaces;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Combat.Net
{
    public class PlayerCombatNet : NetworkBehaviour
    {
        public GameObject[] weapons;

        public IWeapon currentActiveWeapon;

        private weaponType currentWeaponType;

        private bool inTransition;

        private PlayerMovementNet movement;

        public void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
        }

        private void Update()
        {
            if (!hasAuthority) return;
            WeaponSwitcher();
        }

        private void WeaponSwitcher()
        {
            if (inTransition) return;
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponType != weaponType.Grapple)
            {
                SwitchWeapon((int) weaponType.Grapple);
                currentWeaponType = weaponType.Grapple;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponType != weaponType.Glock)
            {
                SwitchWeapon((int) weaponType.Glock);
                currentWeaponType = weaponType.Glock;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //SwitchWeapon((int)weaponType.Deagle);
            }
        }

        private void SwitchWeapon(int _weaponIndex)
        {
            StartCoroutine(TransitionDelay());
            for (var i = 0; i < weapons.Length; i++)
            {
                var isHoldable = weapons[i].TryGetComponent(out IHoldable __activeWeaponSelect);
                if (i == _weaponIndex)
                {
                    weapons[i].SetActive(true);
                    __activeWeaponSelect.AnimateIn();
                    CmdChangeWeaponState(i, true);
                    if (isHoldable)
                    {
                        __activeWeaponSelect.OnSelect();
                        CmdChangeWeaponSelection(i, true);

                        if ((__activeWeaponSelect as MonoBehaviour) is IWeapon)
                            currentActiveWeapon = __activeWeaponSelect as IWeapon;
                        else
                            currentActiveWeapon = null;
                    }

                    continue;
                }

                if (isHoldable)
                {
                    __activeWeaponSelect.OnDeselect();
                    CmdChangeWeaponSelection(i, false);
                }

                weapons[i].SetActive(false);
                CmdChangeWeaponState(i, false);
            }
        }

        private IEnumerator TransitionDelay()
        {
            inTransition = true;
            yield return new WaitForSeconds(0.55f);
            inTransition = false;
        }

        [Command]
        private void CmdChangeWeaponState(int _index, bool _state)
        {
            RpcChangeWeaponState(_index, _state);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcChangeWeaponState(int _index, bool _state)
        {
            weapons[_index].SetActive(_state);
        }

        [Command]
        private void CmdChangeWeaponSelection(int _index, bool _isSelected)
        {
            RpcChangeWeaponSelection(_index, _isSelected);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcChangeWeaponSelection(int _index, bool _isSelected)
        {
            if (weapons[_index].TryGetComponent(out IHoldable __activeWeaponSelect))
            {
                if (_isSelected)
                    __activeWeaponSelect.OnSelect();
                else
                    __activeWeaponSelect.OnDeselect();
            }
        }

        private enum weaponType
        {
            Grapple,
            Glock,
            Awp,
            Shotgun
        }
    }
}