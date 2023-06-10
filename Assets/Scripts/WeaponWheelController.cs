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
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (selected)
        {
        
            itemText.text = itemName;

        }
        
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
