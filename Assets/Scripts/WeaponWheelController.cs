using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{

    public int ID;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Sprite icon;

    private bool selected = false;
    private bool changed = false;
    private Animator anim;
    private Image childIcon;
    //private Sprite finalIcon;
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        childIcon = this.transform.Find("Icon").GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (selected)
        {
        
            itemText.text = itemName;

        }

        checkForChange();
    }

    public void checkForChange()
    {
        if (!changed)
        {
            switch(itemName)
            {
                case "LASER GUN":   
                    changeIcon();
                    break;

                case "FLAMETHROWER":
                    Flamethrower flame = GameObject.Find("Gun").GetComponent<Flamethrower>();
                    if (flame.getEnable())
                    {
                        changeIcon();
                    }
                    break;

                case "BOOMERANG":
                    Boomerang boomerang = GameObject.Find("Gun").GetComponent<Boomerang>();
                    if (boomerang.getEnable())
                    {
                        changeIcon();
                    }
                    break;
            }
        }
    }

    public void changeIcon()
    {
        this.childIcon.sprite = icon;
        this.changed = true;
    }

    public void Selected()
    {
        selected = true;
        WeaponMenuController.weaponID= ID;
    }

    public void Deselected()
    {
        selected= false;
        WeaponMenuController.weaponID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text =itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }


}
