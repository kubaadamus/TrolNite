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
    public List<GameObject> ItemsList;                   //Lista broni
    //public List<GameObject> GunAmmoList;               //Lista ammo
    public GameObject ActiveItem = null;                               //Item który teraz trzymam w ręce
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
        ItemsList = new List<GameObject>();

        ItemsList.Add(Instantiate(Melee, Movement.CurrentItemPosition.transform.position, Movement.CurrentItemPosition.transform.rotation));
        AddGun(DesertEagle);
        ItemSelect();
        SetItemActive();
        HealthUiText.text = Health.ToString();
        FillUiItemsArray();
        MouseUiCursor.transform.localScale = new Vector3(0, 0, 0);
        actualshit = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    }
    // Update is called once per frame
    void Update()
    {
        ItemSelect();                     //Obsługa zmiany broni kółkiem myszy;
        DisplayEquipment();                 //Wyświetlanie ekwipunku
        UseItem();                           //Obsługa strzelania z broni
        DropAllItems();
        DropItem();                          //Obsługa wyrzucania broni
        FindHandMoveVecetor();                  //Określ ruch ręki
        ReloadGun();                        //Obsługa reloadingu
    }
    public void ItemSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && ItemsList.Count > 0) // forward
        {
            if (SelectedItem < ItemsList.Count - 1)
            {
                SetItemInactive();
                SelectedItem++;
                SetItemActive();
                if(ItemsList[SelectedItem].GetComponent<Gun>())
                {
                    //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<Gun>().Type);
                }
                else if(ItemsList[SelectedItem].GetComponent<GunAmmo>())
                {
                    //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<GunAmmo>().ammoType);
                }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && ItemsList.Count > 0) // backwards
        {
            if (SelectedItem > 0)
            {
                SetItemInactive();
                SelectedItem--;
                SetItemActive();
                if (ItemsList[SelectedItem].GetComponent<Gun>())
                {
                    //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<Gun>().Type);
                }
                else if (ItemsList[SelectedItem].GetComponent<GunAmmo>())
                {
                    //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<GunAmmo>().ammoType);
                }
            }
        }
    }
    public void ItemSelect(int _SelectedItem)
    {
        SetItemInactive();
        SelectedItem = _SelectedItem;
        SetItemActive();
        if(ItemsList[SelectedItem].GetComponent<Gun>())
        {
            //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<Gun>().Type);
        }
        else if (ItemsList[SelectedItem].GetComponent<GunAmmo>())
        {
            //Debug.Log("Wybrano " + ItemsList[SelectedItem].GetComponent<GunAmmo>().ammoType);
        }

    }
    public void SetItemActive()
    {
        ItemsList[SelectedItem].transform.position = Movement.CurrentItemPosition.transform.position;
        ItemsList[SelectedItem].transform.rotation = Movement.CurrentItemPosition.transform.rotation;
        ItemsList[SelectedItem].transform.SetParent(Movement.CurrentItemPosition.transform);

        if(ItemsList[SelectedItem].GetComponent<Gun>() )
        {
            ActiveItem = ItemsList[SelectedItem];
            ActiveItem.GetComponent<Animation>().Play("Idle");
            HealthUiText.text = ActiveItem.GetComponent<Gun>().Health.ToString();
            if (ActiveItem.GetComponent<Gun>().Type != GunType.meelee)
            {
                AmmoUiText.text = ActiveItem.GetComponent<Gun>().AmmoLoaded.ToString();
            }
            TypeUiText.text = ActiveItem.GetComponent<Gun>().Type.ToString();
            AudioSourceHandlerScript.PlayAudio(GunSelect, transform.position, 1.0f);
        }
        else if (ItemsList[SelectedItem].GetComponent<GunAmmo>())
        {
            ActiveItem = ItemsList[SelectedItem];
            AudioSourceHandlerScript.PlayAudio(GunSelect, transform.position, 1.0f);
        }
    }
    public void SetItemInactive()
    {
        ItemsList[SelectedItem].transform.position = Movement.BackItemPosition.transform.position;
        ItemsList[SelectedItem].transform.rotation = Movement.BackItemPosition.transform.rotation;
        ItemsList[SelectedItem].transform.SetParent(Movement.BackItemPosition.transform);
        if (ItemsList[SelectedItem].GetComponent<Gun>() && ActiveItem != null)
        {
            if (ActiveItem.GetComponent<Gun>().Type != GunType.meelee)
            {
                AmmoUiText.text = "0";
            }
            ActiveItem = null;
        }
        else if (ItemsList[SelectedItem].GetComponent<GunAmmo>() && ActiveItem != null)
        {
            AmmoUiText.text = "0";
            ActiveItem = null;
        }

    }
    public void DisplayEquipment()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Movement.MouseMovementEnabled = false;
            MouseUiCursor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            MouseUiCursor.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            IsUiActive = true;
            //Debug.Log("BRONIE---------------");
            int itemCount = 0;
            foreach (GameObject Item in ItemsList)
            {
                //Debug.Log("Type: " + Item.GetComponent<Gun>().Type + " / Health: " + Item.GetComponent<Gun>().Health + " / Ammo: " + Item.GetComponent<Gun>().AmmoLoaded);
                //Rysuj UI!
                Button but = Instantiate(_but);
                but.transform.SetParent(_Canvas.transform);
                but.transform.localPosition = UiItemsArray[itemCount];
                but.gameObject.AddComponent<UiButtonItemReferenceScript>().UiButtonGameObjectReference = Item;      //Tworzac każdy guziczek w UI dodaj mu skrypt i podaj mu referencję do obiektu który reprezentuje
                if(Item.GetComponent<Gun>())
                {
                    but.GetComponentInChildren<Text>().text = Item.GetComponent<Gun>().Type.ToString();
                }
                else if (Item.GetComponent<GunAmmo>())
                {
                    but.GetComponentInChildren<Text>().text = Item.GetComponent<GunAmmo>().ammoType.ToString();
                }
                itemCount++;
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
                        //Debug.Log(child.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text);

                        //ustal którym indeksem na liście broni jest to co ma w nazwie string powyżej

                        //InidexOfSelectedGun = ItemsList.FindIndex(f => f.GetComponent<Gun>().Type.ToString() == child.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text);

                        //Ustal do którego obiektu w ItemLiście wskazuje referencja pokazana w UI
                        foreach(GameObject Item in ItemsList)
                        {
                            if(child.gameObject.GetComponent<UiButtonItemReferenceScript>().UiButtonGameObjectReference == Item)
                            {
                                InidexOfSelectedGun = ItemsList.IndexOf(Item);
                            }
                        }


                        //Debug.Log("INDEKS ZAZNACZONEJ BRONI TO:" + InidexOfSelectedGun);
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
            ItemSelect(InidexOfSelectedGun);
        }
    }
    public void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(ActiveItem);
            if (ActiveItem.GetComponent<Gun>())
            {
                ActiveItem.GetComponent<Gun>().Shot();
                if (ActiveItem.GetComponent<Gun>().Type != GunType.meelee)
                {
                    AmmoUiText.text = ActiveItem.GetComponent<Gun>().AmmoLoaded.ToString();
                }
            }
            if (ActiveItem.GetComponent<GunAmmo>() && ActiveItem.GetComponent<GunAmmo>().ammoType==GunAmmoType.medikit)
            {
                Health += ActiveItem.GetComponent<GunAmmo>().ammoAmount;
                Debug.Log("Podleczam: " + Health);
                GameObject ItemToRemove = ActiveItem;
                SetItemInactive();
                SelectedItem = 0;
                SetItemActive();
                ItemsList.Remove(ItemToRemove);
                Destroy(ItemToRemove);
            }
        }
    }
    public void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ItemsList[SelectedItem].GetComponent<Gun>() && ItemsList[SelectedItem].GetComponent<Gun>().Type != GunType.meelee)
            {
                ItemsList[SelectedItem].GetComponent<Rigidbody>().isKinematic = false;
                ItemsList[SelectedItem].GetComponent<Rigidbody>().useGravity = true;
                ItemsList[SelectedItem].transform.SetParent(null);

                //Debug.Log("Wyrzucono: " + ItemsList[SelectedItem].GetComponent<Gun>().Type);
                ItemsList.Remove(ItemsList[SelectedItem]);
                SelectedItem = 0;
                SetItemActive();
            }
            else if(ItemsList[SelectedItem].GetComponent<GunAmmo>())
            {
                ItemsList[SelectedItem].GetComponent<Rigidbody>().isKinematic = false;
                ItemsList[SelectedItem].GetComponent<Rigidbody>().useGravity = true;
                ItemsList[SelectedItem].transform.SetParent(null);

                //Debug.Log("Wyrzucono: " + ItemsList[SelectedItem].GetComponent<GunAmmo>().ammoType);
                ItemsList.Remove(ItemsList[SelectedItem]);
                SelectedItem = 0;
                SetItemActive();
            }

        }
    }
    public void DropAllItems()
    {
        List<GameObject> ItemsToRemove = new List<GameObject>();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach(GameObject Item in ItemsList)
            {
                if (Item.GetComponent<Gun>() && Item.GetComponent<Gun>().Type != GunType.meelee)
                {
                    Item.GetComponent<Rigidbody>().isKinematic = false;
                    Item.GetComponent<Rigidbody>().useGravity = true;
                    Item.transform.SetParent(null);

                    //Debug.Log("Wyrzucono: " + ItemsList[SelectedItem].GetComponent<Gun>().Type);
                    ItemsToRemove.Add(Item);
                    SelectedItem = 0;
                    SetItemActive();
                }
                else if (Item.GetComponent<GunAmmo>())
                {
                    Item.GetComponent<Rigidbody>().isKinematic = false;
                    Item.GetComponent<Rigidbody>().useGravity = true;
                    Item.transform.SetParent(null);

                    //Debug.Log("Wyrzucono: " + ItemsList[SelectedItem].GetComponent<GunAmmo>().ammoType);
                    ItemsToRemove.Add(Item);
                    SelectedItem = 0;
                    SetItemActive();
                }
            }
        }
        foreach(GameObject item in ItemsToRemove) ItemsList.Remove(item);
    }
    public void ReloadGun()
    {

        if (Input.GetKeyDown(KeyCode.R) && ActiveItem!=null && ActiveItem.GetComponent<Gun>() && ActiveItem.GetComponent<Gun>().Type != GunType.meelee)
        {
            //sprawdz typ broni ktora trzymasz
            GunType HeldWeaponType = ActiveItem.GetComponent<Gun>().Type;
            //sprawdz czy masz ammo w schowku
            GameObject MatchedAmmoType = null;
            foreach (GameObject Item in ItemsList)
            {
                if(Item.GetComponent<GunAmmo>())
                {
                    if (Item.GetComponent<GunAmmo>().ammoType.ToString() == HeldWeaponType.ToString() && Item.GetComponent<GunAmmo>().ammoAmount > 0)
                    {
                        MatchedAmmoType = Item;
                            //Debug.Log("Mam w ekwipunku odpowiednie ammo w ilości: " + Item.GetComponent<GunAmmo>().ammoAmount);
                    }
                    else
                    {
                        //Debug.Log("Nie mam ammo do tej broni :/");
                    }
                }
            }
            if (MatchedAmmoType != null && ActiveItem.GetComponent<Gun>().AmmoLoaded < ActiveItem.GetComponent<Gun>().MaxAmmo)
            {
                //le potrzeba amunicji żeby na max przeładować guna?
                int NeededAmmo = ActiveItem.GetComponent<Gun>().MaxAmmo - ActiveItem.GetComponent<Gun>().AmmoLoaded;

                if (NeededAmmo >= MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)  //Jeśli potrzeba więcej lub równo tego co mamy w kieszeni to wysyłamy gunowi wszystko
                {
                    ActiveItem.GetComponent<Gun>().Reload(MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount = 0;

                }
                else if (NeededAmmo < MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount)   
                {
                    ActiveItem.GetComponent<Gun>().Reload(ActiveItem.GetComponent<Gun>().MaxAmmo - ActiveItem.GetComponent<Gun>().AmmoLoaded);
                    MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount -= NeededAmmo;
                }
                //Debug.Log("Reloaded! pozostało wolnej amunicji" + MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount);
                AmmoUiText.text = ActiveItem.GetComponent<Gun>().AmmoLoaded.ToString();
                if (MatchedAmmoType.GetComponent<GunAmmo>().ammoAmount == 0)
                {
                    ItemsList.Remove(MatchedAmmoType);
                    Destroy(MatchedAmmoType);
                }
            }
        }

    }
    public void FillUiItemsArray()
    {
        int i = 0;
        for (int y = 0; y < 5; y++)
        {
            
            for(int x=0; x<5; x++)
            {
                UiItemsArray[i] = new Vector3(x*55-100,-y*55+100, 0);
                i++;
            }

        }
    }
    public void AddGun(GameObject GunToAdd)
    {
        GameObject AddedGun = Instantiate(GunToAdd, Movement.BackItemPosition.transform.position, Movement.BackItemPosition.transform.rotation);
        AddedGun.GetComponent<Rigidbody>().isKinematic = true;
        AddedGun.GetComponent<Rigidbody>().useGravity = false;
        //Debug.Log("Podniesiono " + AddedGun.GetComponent<Gun>().Type + " health:" + AddedGun.GetComponent<Gun>().Health + " ammo: " + AddedGun.GetComponent<Gun>().AmmoLoaded);
        AddedGun.transform.position = Movement.BackItemPosition.transform.position;
        AddedGun.transform.rotation = Movement.BackItemPosition.transform.rotation;
        AddedGun.transform.SetParent(Movement.BackItemPosition.transform);
        AudioSourceHandlerScript.PlayAudio(Movement.GunPickUpAudioClip, transform.position, 1.0f);
        ItemsList.Add(AddedGun);
    }

}


