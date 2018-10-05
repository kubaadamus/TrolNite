using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunAmmoType { meelee, pistol, shotgun, rifle, sniper, granade, mine };
public class GunAmmo : MonoBehaviour{
    public GunAmmoType ammoType;
    public int ammoAmount;
    public GunAmmo(GunAmmoType _ammoType, int _ammoAmount)
    {
        ammoType = _ammoType;
        ammoAmount = _ammoAmount;
    }
}
