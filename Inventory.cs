using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Interact interact;
    [SerializeField] private Image[] icons;
    [SerializeField] private TMP_Text[] text;
    public void Set(float i)
    {
        interact.i = i;
    }

    private void FixedUpdate()
    {
        text[0].text = interact.inventory[0].ToString();
        text[1].text = interact.inventory[1].ToString();
        text[2].text = interact.inventory[2].ToString();
    }

}
