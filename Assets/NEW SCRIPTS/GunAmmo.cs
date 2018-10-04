using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunAmmoType { pistol_ammo, shotgun_ammo, rifle_ammo, sniper_ammo, granade_ammo, mine_ammo };
public class GunAmmoItem{
    public GunAmmoType ammoType;
    public int ammoAmount;
    public GunAmmoItem(GunAmmoType _ammoType, int _ammoAmount)
    {
        ammoType = _ammoType;
        ammoAmount = _ammoAmount;
    }

}
