using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    public GameObject Melee;
    public GameObject DesertEagle;
    public Text AmmoUiText;
    public Text HealthUiText;
    public Text TypeUiText;
    public Image MouseUiCursor;
    Vector3 MouseTransform = new Vector3(0, 0, 0);
    Vector3 actualshit = new Vector3(950, 450, 0);
    int InidexOfSelectedGun = 0;
    Button Descriptionbut = null;
    public AudioClip GunSelect;
    public bool IsUiActive = false;
    public Vector3[] UiItemsArray = new Vector3[30];
    public Button _but;
    public Canvas _Canvas;
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
        AddGun(DesertEagle);
        WeaponSelect();
        GunAmmoList = new List<GameObject>();
        SetGunActive();
        HealthUiText.text = Health.ToString();
        FillUiItemsArray();
        MouseUiCursor.transform.localScale = new Vector3(0, 0, 0);
        actualshit = new Vector3(Screen.width / 2, Screen.height / 2, 0);

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
    public void WeaponSelect(int SelectedGun)
    {
        SetGunInactive();
        SelectedItem = SelectedGun;
        SetGunActive();
        Debug.Log("Wybrano " + GunsList[SelectedItem].GetComponent<Gun>().Type);
    }
    public void SetGunActive()
    {
        GunsList[SelectedItem].transform.position = Movement.CurrentItemPosition.transform.position;
        GunsList[SelectedItem].transform.rotation = Movement.CurrentItemPosition.transform.rotation;
        GunsList[SelectedItem].transform.SetParent(Movement.CurrentItemPosition.transform);
        ActiveGun = GunsList[SelectedItem].GetComponent<Gun>();
        ActiveGun.GetComponent<Animation>().Play("Idle");
        HealthUiText.text = ActiveGun.Health.ToString();
        if(ActiveGun.Type!=GunType.meelee)
        {
            AmmoUiText.text = ActiveGun.AmmoLoaded.ToString();
        }

        TypeUiText.text = ActiveGun.Type.ToString();
        AudioSourceHandlerScript.PlayAudio(GunSelect, transform.position, 1.0f);
    }
    public void SetGunInactive()
    {
        GunsList[SelectedItem].transform.position = Movement.BackItemPosition.transform.position;
        GunsList[SelectedItem].transform.rotation = Movement.BackItemPosition.transform.rotation;
        GunsList[SelectedItem].transform.SetParent(Movement.CurrentItemPosition.transform);
        if (ActiveGun.Type != GunType.meelee)
        {
            AmmoUiText.text = "0";
        }
        ActiveGun = null;

    }
    public void DisplayEquipment()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Movement.MouseMovementEnabled = false;
            MouseUiCursor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            MouseUiCursor.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            IsUiActive = true;
            Debug.Log("BRONIE---------------");
            int itemCount = 0;
            foreach (GameObject gun in GunsList)
            {
                Debug.Log("Type: " + gun.GetComponent<Gun>().Type + " / Health: " + gun.GetComponent<Gun>().Health + " / Ammo: " + gun.GetComponent<Gun>().AmmoLoaded);
                //Rysuj UI!
                Button but = Instantiate(_but);
                but.transform.SetParent(_Canvas.transform);
                but.transform.localPosition = UiItemsArray[itemCount];
                but.GetComponentInChildren<Text>().text = gun.GetComponent<Gun>().Type.ToString();
                itemCount++;
            }
            Debug.Log("AMMO-----------------");
            foreach (GameObject ammo in GunAmmoList)
            {
                Debug.Log("Type: " + ammo.GetComponent<GunAmmo>().ammoType + " / Amount: " + ammo.GetComponent<GunAmmo>().ammoAmount);
                //Rysuj UI!
                //Button but = Instantiate(_but);
                //but.transform.SetParent(_Canvas.transform);
                //but.transform.localPosition = UiItemsArray[itemCount];
                //but.GetComponentInChildren<Text>().text = ammo.GetComponent<GunAmmo>().ammoType.ToString() + "AMMO";
                //itemCount++;
            }

        }
        if (IsUiActive)
        {

            MouseTransform = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * 10;
            //Debug.Log(MouseUiCursor.transform.position);
            actualshit += MouseTransform;
            if (actualshit.x < Screen.width / 2 - 200)
            {
                actualshit = new Vector3(Screen.width / 2 - 200, actualshit.y, actualshit.z);
            }
            if (actualshit.x > Screen.width / 2 + 200)
            {
                actualshit = new Vector3(Screen.width / 2 + 200, actualshit.y, actualshit.z);
            }
            if (actualshit.y > Screen.height / 2 + 200)
            {
                actualshit = new Vector3(actualshit.x, Screen.height / 2 + 200, actualshit.z);
            }
            if (actualshit.y < Screen.height / 2 - 200)
            {
                actualshit = new Vector3(actualshit.x, Screen.height / 2 - 200, actualshit.z);
            }
            //Debug.Log(actualshit);
            MouseUiCursor.transform.position = actualshit;
            //Jesli pozycja tego tymczasowego kursora jest bliżej do któregoś buttona niż określony MinDistance to aktywuj tego buttona
            foreach (Transform child in _Canvas.transform)
            {
                if (child.gameObject.name.Contains("Button"))
                {
                    if (Vector3.Distance(child.gameObject.transform.position, MouseUiCursor.transform.position) < 25)
                    {
                        child.gameObject.GetComponent<Button>().GetComponent<Image>().color = Color.red;
                        Debug.Log(child.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text);

                        //ustal którym indeksem na liście broni jest to co ma w nazwie string powyżej

                        InidexOfSelectedGun = GunsList.FindIndex(f => f.GetComponent<Gun>().Type.ToString() == child.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text);
                        Debug.Log("INDEKS ZAZNACZONEJ BRONI TO:" + InidexOfSelectedGun);
                    }
                    else
                    {
                        child.gameObject.GetComponent<Button>().GetComponent<Image>().color = Color.white;
                    }

                }
                else
                {

                }
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            Movement.MouseMovementEnabled = true;
            actualshit = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            IsUiActive = false;
            foreach (Transform child in _Canvas.transform)
            {
                if (child.gameObject.name.Contains("Button") && !child.gameObject.name.Contains("Empty"))
                {
                    GameObject.Destroy(child.gameObject);
                }

            }
            MouseUiCursor.transform.localPosition = new Vector3(0, 0, 0);
            MouseUiCursor.transform.localScale = new Vector3(0, 0, 0);
            WeaponSelect(InidexOfSelectedGun);
        }
    }
    public void UseGun()
    {
        if (Input.GetMouseButtonDown(0) && ActiveGun != null)
        {
            ActiveGun.Shot();
            if(ActiveGun.Type != GunType.meelee)
            {
                AmmoUiText.text = ActiveGun.AmmoLoaded.ToString();
            }

        }
    }
    public void DropGun()
    {
        if (Input.GetKeyDown(KeyCode.X) && GunsList[SelectedItem].GetComponent<Gun>().Type != GunType.meelee)
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
        if (Input.GetKeyDown(KeyCode.R) && ActiveGun.Type!=GunType.meelee)
        {
            //sprawdz typ broni ktora trzymasz
            GunType HeldWeaponType = ActiveGun.Type;
            //sprawdz czy masz ammo w schowku
            GameObject MatchedAmmoType = null;
            foreach (GameObject ammo in GunAmmoList)
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
            if (MatchedAmmoType != null && ActiveGun.AmmoLoaded < ActiveGun.MaxAmmo)
            {
                //le potrzeba amunicji żeby na max przeładować guna?
                int NeededAmmo = ActiveGun.MaxAmmo - ActiveGun.AmmoLoaded;

                if (NeededAmmo >= MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)  //Jeśli potrzeba więcej lub równo tego co mamy w kieszeni to wysyłamy gunowi wszystko
                {
                    ActiveGun.Reload(MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount = 0;
                }
                if (NeededAmmo < MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)   //Jesli potrzeba mniej niż mamy w kieszeni to wyjmij tyle ile trzeba i wyslij gunowi
                {
                    ActiveGun.Reload(ActiveGun.MaxAmmo - ActiveGun.AmmoLoaded);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount -= ActiveGun.MaxAmmo - ActiveGun.AmmoLoaded;
                }
                Debug.Log("Reloaded! pozostało wolnej amunicji" + MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                AmmoUiText.text = ActiveGun.AmmoLoaded.ToString();
                if (MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount == 0)
                {
                    GunAmmoList.Remove(MatchedAmmoType);
                    Destroy(MatchedAmmoType);
                }
            }
        }

    }
    public void FillUiItemsArray()
    {
        for (int i = 0; i < 30; i++)
        {
            UiItemsArray[i] = new Vector3(i * 55 - 100, 0, 0);
        }
    }
    public void AddGun(GameObject GunToAdd)
    {
        GameObject AddedGun = Instantiate(GunToAdd, Movement.BackItemPosition.transform.position, Movement.BackItemPosition.transform.rotation);
        AddedGun.GetComponent<Rigidbody>().isKinematic = true;
        AddedGun.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log("Podniesiono " + AddedGun.GetComponent<Gun>().Type + " health:" + AddedGun.GetComponent<Gun>().Health + " ammo: " + AddedGun.GetComponent<Gun>().AmmoLoaded);
        AddedGun.transform.position = Movement.BackItemPosition.transform.position;
        AddedGun.transform.rotation = Movement.BackItemPosition.transform.rotation;
        AddedGun.transform.SetParent(Movement.BackItemPosition.transform);
        AudioSourceHandlerScript.PlayAudio(Movement.GunPickUpAudioClip, transform.position, 1.0f);
        GunsList.Add(AddedGun);
    }

}


