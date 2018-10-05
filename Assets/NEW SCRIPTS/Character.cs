using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Character : MonoBehaviour
{
    public GameObject Melee;
    public string NazwaGracza = "";                     //Nazwa gracza
    public int Health = 50;                             //Zycie
    public List<GameObject> GunsList;                   //Lista broni
    public List<GameObject> GunAmmoList;               //Lista ammo
    public Gun ActiveGun = null;                               //Item który teraz trzymam w ręce
    public int SelectedItem = 0;                        //Indeks itemu który trzymam
    CharacterMovement Movement;
    Vector3 PreviousHandPosition;                       //Poprzednia pozycja ręki do określania szybkości i kierunku rzutu ręką
    Vector3 HandsMoveVector;                            //Wektor mówiący gdzie i jak mocno macha ręka
    void FindHandMoveVecetor()
    {
        HandsMoveVector = ((Movement.CurrentItemPosition.transform.position - PreviousHandPosition)) / Time.deltaTime;
        PreviousHandPosition = Movement.CurrentItemPosition.transform.position;
    }
    void Start()
    {
        Movement = GetComponent<CharacterMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        GunsList = new List<GameObject>();
        GunsList.Add(Instantiate(Melee, Movement.CurrentItemPosition.transform.position, Movement.CurrentItemPosition.transform.rotation));
        WeaponSelect();
        GunAmmoList = new List<GameObject>();
        SetGunActive();

    }
    // Update is called once per frame
    void Update()
    {
        WeaponSelect();                     //Obsługa zmiany broni kółkiem myszy;
        DisplayEquipment();                 //Wyświetlanie ekwipunku
        UseGun();                           //Obsługa strzelania z broni
        DropGun();                          //Obsługa wyrzucania broni
        FindHandMoveVecetor();                  //Określ ruch ręki
        ReloadGun();                        //Obsługa reloadingu
    }
    public void WeaponSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && GunsList.Count > 0) // forward
        {
            if (SelectedItem < GunsList.Count - 1)
            {
                SetGunInactive();
                SelectedItem++;
                SetGunActive();
                Debug.Log("Wybrano " + GunsList[SelectedItem].GetComponent<Gun>().Type);

            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && GunsList.Count > 0) // backwards
        {
            if (SelectedItem > 0)
            {
                SetGunInactive();
                SelectedItem--;
                SetGunActive();
                Debug.Log("Wybrano " + GunsList[SelectedItem].GetComponent<Gun>().Type);
            }
        }
    }
    public void SetGunActive()
    {
        GunsList[SelectedItem].transform.position = Movement.CurrentItemPosition.transform.position;
        GunsList[SelectedItem].transform.rotation = Movement.CurrentItemPosition.transform.rotation;
        GunsList[SelectedItem].transform.SetParent(Movement.CurrentItemPosition.transform);
        ActiveGun = GunsList[SelectedItem].GetComponent<Gun>();
        ActiveGun.GetComponent<Animation>().Play("Idle");
    }
    public void SetGunInactive()
    {
        GunsList[SelectedItem].transform.position = Movement.BackItemPosition.transform.position;
        GunsList[SelectedItem].transform.rotation = Movement.BackItemPosition.transform.rotation;
        GunsList[SelectedItem].transform.SetParent(Movement.CurrentItemPosition.transform);
        ActiveGun = null;
    }
    public void DisplayEquipment()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("BRONIE---------------");
            foreach (GameObject gun in GunsList)
            {
                Debug.Log("Type: " + gun.GetComponent<Gun>().Type + " / Health: " + gun.GetComponent<Gun>().Health + " / Ammo: " + gun.GetComponent<Gun>().AmmoLoaded);
            }
            Debug.Log("AMMO-----------------");
            foreach (GameObject ammo in GunAmmoList)
            {
                Debug.Log("Type: " + ammo.GetComponent<GunAmmo>().ammoType + " / Amount: " + ammo.GetComponent<GunAmmo>().ammoAmount);
            }
        }
    }
    public void UseGun()
    {
        if (Input.GetMouseButtonDown(0) && ActiveGun != null)
        {
            ActiveGun.Shot();
        }
    }
    public void DropGun()
    {
        if (Input.GetKeyDown(KeyCode.X) && GunsList[SelectedItem].GetComponent<Gun>().Type!=GunType.meelee)
        {
            GunsList[SelectedItem].GetComponent<Rigidbody>().isKinematic = false;
            GunsList[SelectedItem].GetComponent<Rigidbody>().useGravity = true;
            GunsList[SelectedItem].transform.SetParent(null);

            Debug.Log("Wyrzucono: " + GunsList[SelectedItem].GetComponent<Gun>().Type);
            GunsList.Remove(GunsList[SelectedItem]);
            SelectedItem = 0;
            SetGunActive(); 
        }
    }
    public void ReloadGun()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            //sprawdz typ broni ktora trzymasz
            GunType HeldWeaponType = ActiveGun.Type;
            //sprawdz czy masz ammo w schowku
            GameObject MatchedAmmoType = null;
            foreach(GameObject ammo in GunAmmoList)
            {
                if (ammo.GetComponent<GunAmmo>().ammoType.ToString() == HeldWeaponType.ToString())
                {
                    MatchedAmmoType = ammo;
                    Debug.Log("Znalazlem odpowiednie ammo w ilosci: " + ammo.GetComponent<GunAmmo>().ammoAmount);
                }
                else
                {
                    Debug.Log("Nie mam ammo do tej broni :C");
                }
            }
            if(MatchedAmmoType != null && ActiveGun.AmmoLoaded<ActiveGun.MaxAmmo)
            {
                //le potrzeba amunicji żeby na max przeładować guna?
                int NeededAmmo = ActiveGun.MaxAmmo - ActiveGun.AmmoLoaded;

                if(NeededAmmo>=MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)  //Jeśli potrzeba więcej lub równo tego co mamy w kieszeni to wysyłamy gunowi wszystko
                {
                    ActiveGun.Reload(MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount = 0;
                }
                if(NeededAmmo<MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)   //Jesli potrzeba mniej niż mamy w kieszeni to wyjmij tyle ile trzeba i wyslij gunowi
                {
                    ActiveGun.Reload(ActiveGun.MaxAmmo- ActiveGun.AmmoLoaded);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount -= ActiveGun.MaxAmmo - ActiveGun.AmmoLoaded;
                }
                Debug.Log("Reloaded! pozostało wolnej amunicji" + MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                if (MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount == 0)
                {
                    GunAmmoList.Remove(MatchedAmmoType);
                    Destroy(MatchedAmmoType);
                }
            }
        }
    }
}


