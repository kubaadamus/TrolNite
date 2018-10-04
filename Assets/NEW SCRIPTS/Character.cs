using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public string NazwaGracza = "";                     //Nazwa gracza
    public int Health = 50;                             //Zycie
    public List<GunItem> GunsList;                      //Lista broni
    public List<GunAmmoItem> GunAmmoList;                   //Lista ammo
    public GameObject ActiveItem;                       //Item który teraz trzymam w ręce
    public int SelectedItem = 0;                        //Indeks itemu który trzymam



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GunsList = new List<GunItem>();
        GunAmmoList = new List<GunAmmoItem>();

        //GunsList.Add(new GunItem(GunType.pistol,100));                   //Dodaj sobie pistola na wstępie
        //GunAmmoList.Add(new GunAmmoItem(GunAmmoType.pistol_ammo, 100));    //Dodaj sobie objekt 100 sztuka municji do pistola
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSelect();                     //Obsługa zmiany broni kółkiem myszy;
    }
    public void WeaponSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && GunsList.Count > 0) // forward
        {
            if (SelectedItem < GunsList.Count)
            {
                SelectedItem++;
                Debug.Log("Wybrano item: " + SelectedItem + " " + GunsList[SelectedItem - 1].Type + " ammo: " + GunsList[SelectedItem - 1].GunAmmoLoaded + " health: " + GunsList[SelectedItem - 1].GunHealth);
            }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && GunsList.Count > 0) // backwards
        {
            if (SelectedItem > 1)
            {
                SelectedItem--;
                Debug.Log("Wybrano item: " + SelectedItem + " " + GunsList[SelectedItem - 1].Type + " ammo: " + GunsList[SelectedItem - 1].GunAmmoLoaded + " health: " + GunsList[SelectedItem - 1].GunHealth);
            }
        }
    }
}


