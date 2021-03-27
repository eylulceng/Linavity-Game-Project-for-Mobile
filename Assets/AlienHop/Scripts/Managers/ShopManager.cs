using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    public GameObject scrollContent, shopItemPrefab, shopItemName, shopMenu, shopPlay, shopBuy;
    public Button closeShop, shopSelectButton, openShop;
    public Text shopSelectButtonText;
    public ScrollRect shopScroll;
    public string shopState;
    public ScrollRect scroll;
    public int scrollItemWidth, characterIndex, unlockableItemIndex;
    public managerVars vars;
    public Image starImg;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
    }

    // Use this for initialization
    void Start ()
    {
        scrollItemWidth = 200;//width of our character image (the prefabs which contain image component to show character image)
        closeShop.GetComponent<Button>().onClick.AddListener(() => { CloseShopMenu(); });
        openShop.GetComponent<Button>().onClick.AddListener(() => { OpenShopMenu(); });
        shopSelectButton.GetComponent<Button>().onClick.AddListener(() => { SelectCharacter(); });
        starImg.sprite = vars.starImg;
        shopPlay.GetComponent<Image>().sprite = vars.playButton;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //current Location
        float curLoc = scroll.content.anchoredPosition.x / scrollItemWidth;
        //location to rach
        float locToReach = Mathf.Floor(curLoc);
        float posBetween = locToReach - curLoc;
        float type62 = posBetween * scrollItemWidth;

        // Update Pos
        if (Input.GetMouseButtonUp(0))
        {
            if (type62 >= -(scrollItemWidth / 2) + 1)
            {
                scroll.content.anchoredPosition = new Vector2(-Mathf.Floor(curLoc) * -scrollItemWidth, 0f);
            }
            else if (type62 <= -(scrollItemWidth / 2))
            {
                scroll.content.anchoredPosition = new Vector2(-Mathf.Ceil(curLoc) * -scrollItemWidth, 0f);
            }
        }

        // Update Index
        if (type62 >= -(scrollItemWidth / 2) + 1)
        {
            characterIndex = Mathf.Abs(Mathf.FloorToInt(curLoc));
        }
        else if (type62 <= -(scrollItemWidth / 2))
        {
            characterIndex = Mathf.Abs(Mathf.CeilToInt(curLoc));
        }
        //check if shop menu is active
        if (shopMenu.activeSelf)
        {   //if yes then we set the position of character images 
            for (int i = 0; i <= scrollContent.transform.childCount - 1; i++)
            {   //we make the selected image size to its full size
                if (i == characterIndex)
                {
                    scrollContent.transform.GetChild(characterIndex).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                else //and we make the un-selected image size to its half size
                {
                    scrollContent.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 60f);
                }
            }
            //we then sets the name of character
            shopItemName.GetComponent<Text>().text = vars.characters[characterIndex].characterName;
            //set the button 
            if (GameManager.instance.skinUnlocked[characterIndex] == true)
            {
                shopPlay.SetActive(true);
                shopBuy.SetActive(false);
            }
            else if (GameManager.instance.skinUnlocked[characterIndex] == false)
            {
                shopPlay.SetActive(false);
                shopBuy.SetActive(true);
                shopSelectButtonText.text = "" + vars.characters[characterIndex].characterPrice;
            }
        }

    }// Update

    //method called to open the shop
    public void OpenShopMenu()
    {
        UIObjects.instance.ButtonPress();
        GameUI.instance.mainMenu.SetActive(false);
        shopMenu.SetActive(true);
        UpdateShopItems();
    }

    //method called to close the shop
    public void CloseShopMenu()
    {
        UIObjects.instance.ButtonPress();
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        shopMenu.SetActive(false);
        GameUI.instance.mainMenu.SetActive(true);
    }

    //method called by select button to select the character
    public void SelectCharacter()
    {
        UIObjects.instance.ButtonPress();
        if (GameManager.instance.skinUnlocked[characterIndex] == true)
        {
            GameManager.instance.selectedSkin = characterIndex;
            GameManager.instance.Save();
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
        else if (GameManager.instance.points >= vars.characters[characterIndex].characterPrice)
        {
            GameManager.instance.points -= vars.characters[characterIndex].characterPrice;
            GameManager.instance.skinUnlocked[characterIndex] = true;
            GameManager.instance.selectedSkin = characterIndex;
            GameManager.instance.Save();

            //we 1st destroy all the gameobjects
            foreach (Transform child in scrollContent.transform)
            {
                Destroy(child.gameObject);
            }
            //then update the shop
            UpdateShopItems();

        }
        else if (GameManager.instance.points < vars.characters[characterIndex].characterPrice)
        {
            Debug.Log("Buy Coins");
        }
    }

    //method which controls the movement and scrolling and spawning image prefabs 
    public void UpdateShopItems()
    {   //set the scrollContent parent size
        scrollContent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.characters.Count * scrollItemWidth) + 300f, 250f);
        //set the scrollContent size
        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.characters.Count * scrollItemWidth) + 300f, 150f);
        //loop through all the characters
        for (int i = 0; i <= (vars.characters.Count - 1); i++)
        {   //spawn the prefab
            GameObject shopItem = Instantiate(shopItemPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            shopItem.transform.SetParent(scrollContent.transform);//set its parent
            shopItem.transform.localRotation = Quaternion.Euler(0, 0, 0);//set its rotation
            shopItem.transform.localScale = new Vector3(1, 1, 1);//set its scale
            if (i == 0) //the prefas at 0 index
            {   //set its position
                shopItem.transform.localPosition = new Vector3(190f, 0, 0);
            }
            else
            {   //set other next prefabs position
                shopItem.transform.localPosition = new Vector3((scrollItemWidth * i) + 190f, 0, 0);
            }
            //we get the tranform of the object which has Image component on it(image prefab)
            Transform x = shopItem.transform.GetChild(0).GetChild(0);
            if (i == 0)
            {   //set the image value
                x.GetComponent<Image>().sprite = vars.characters[0].shopCharacterSprite;
            }
            else
            {   //set the image value
                x.GetComponent<Image>().sprite = vars.characters[i].shopCharacterSprite;
                if (GameManager.instance.skinUnlocked[i] == false)
                {   //if the characters are unlocked then there color is set
                    x.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                    //x.GetComponent<Image>().color = new Color32(0, 0, 0, 150);
                }
            }
        }
        //when we open the shop the scroll is set to show the selected object in middle
        shopScroll.content.anchoredPosition = new Vector2(-(GameManager.instance.selectedSkin * scrollItemWidth), 0f);
    }

}//class
