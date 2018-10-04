using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Character : MonoBehaviour
{
    //Dostęp do prefabów//
    public Gun Melee;
    public Gun Pistol;
    public Gun Shotgun;
    public Gun InstantiatedGun;

    public string NazwaGracza = "";                     //Nazwa gracza
    public int Health = 50;                             //Zycie
    public List<GunItem> GunsList;                      //Lista broni
    public List<GunAmmoItem> GunAmmoList;                   //Lista ammo
    public Gun ActiveGun;                       //Item który teraz trzymam w ręce
    public int SelectedItem = 0;                        //Indeks itemu który trzymam
    CharacterMovement Movement;
    void Start()
    {
        Movement = GetComponent<CharacterMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        GunsList = new List<GunItem>();
        GunAmmoList = new List<GunAmmoItem>();
        GunsList.Add(new GunItem(GunType.meelee,100));                       //Dodaj sobie pistola na wstępie
        //GunAmmoList.Add(new GunAmmoItem(GunAmmoType.pistol_ammo, 100));    //Dodaj sobie objekt 100 sztuka municji do pistola
    }
    // Update is called once per frame
    void Update()
    {
        WeaponSelect();                     //Obsługa zmiany broni kółkiem myszy;
        DisplayEquipment();                 //Wyświetlanie ekwipunku
        UseGun();                           //Obsługa strzelania z broni
        DropGun();                          //Obsługa wyrzucania broni
    }
    public void WeaponSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && GunsList.Count > 0) // forward
        {
            if (SelectedItem < GunsList.Count-1)
            {
                SelectedItem++;
                Debug.Log("Wybrano item: " + SelectedItem + " " + GunsList[SelectedItem].Type + " ammo: " + GunsList[SelectedItem].GunAmmoLoaded + " health: " + GunsList[SelectedItem].GunHealth);
                InstantiateGun(GunsList[SelectedItem].Type, GunsList[SelectedItem].GunAmmoLoaded, GunsList[SelectedItem].GunHealth);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && GunsList.Count > 0) // backwards
        {
            if (SelectedItem > 0)
            {
                SelectedItem--;
                Debug.Log("Wybrano item: " + SelectedItem + " " + GunsList[SelectedItem].Type + " ammo: " + GunsList[SelectedItem].GunAmmoLoaded + " health: " + GunsList[SelectedItem].GunHealth);
                InstantiateGun(GunsList[SelectedItem].Type, GunsList[SelectedItem].GunAmmoLoaded, GunsList[SelectedItem].GunHealth);
            }
        }
    }
    public void InstantiateGun(GunType _type, int _ammo, int _health)
    {
        ActiveGun = null;
        foreach (Transform child in Movement.CurrentItemPosition.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        switch (_type)
        {
            case GunType.meelee:
                InstantiatedGun = Instantiate(Melee, Movement.CurrentItemPosition.transform.position, Movement.CurrentItemPosition.transform.rotation);
                break;
            case GunType.pistol:
                InstantiatedGun = Instantiate(Pistol, Movement.CurrentItemPosition.transform.position, Movement.CurrentItemPosition.transform.rotation);
                break;
            case GunType.shotgun:
                InstantiatedGun = Instantiate(Shotgun, Movement.CurrentItemPosition.transform.position, Movement.CurrentItemPosition.transform.rotation);
                break;
        }
        InstantiatedGun.Type = GunsList[SelectedItem].Type;
        InstantiatedGun.Health = GunsList[SelectedItem].GunHealth;
        InstantiatedGun.AmmoLoaded = GunsList[SelectedItem].GunAmmoLoaded;

        InstantiatedGun.transform.SetParent(Movement.CurrentItemPosition.transform);
        InstantiatedGun.GetComponent<Rigidbody>().isKinematic = true;
        InstantiatedGun.GetComponent<Rigidbody>().useGravity = false;
        ActiveGun = InstantiatedGun;
    }
    public void DisplayEquipment()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("BRONIE---------------");
            foreach(GunItem gun in GunsList)
            {
                Debug.Log("Type: " + gun.Type + " / Health: " + gun.GunHealth + " / Ammo: " + gun.GunAmmoLoaded);
            }
            Debug.Log("AMMO-----------------");
            foreach (GunAmmoItem ammo in GunAmmoList)
            {
                Debug.Log("Type: " + ammo.ammoType + " / Amount: " + ammo.ammoAmount);
            }
        }
    }
    public void UseGun()
    {
        if (Input.GetMouseButtonDown(0) && ActiveGun!=null)
        {
            ActiveGun.Shot();
        }
    }
    public void DropGun()
    {
        if (Input.GetKeyDown(KeyCode.X) && ActiveGun!=null && ActiveGun.Type!=GunType.meelee)
        {
            ActiveGun.GetComponent<Rigidbody>().isKinematic = false;
            ActiveGun.GetComponent<Rigidbody>().useGravity = true;
            ActiveGun.transform.SetParent(null);
            var itemToRemove = GunsList.Single(r => r.Type == ActiveGun.Type);
            Debug.Log("Wyrzucono" + itemToRemove.Type);
            GunsList.Remove(itemToRemove);

        }
    }
}


