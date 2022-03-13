using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Interact interact;
    [SerializeField] private Image[] icons;
    [SerializeField] private TMP_Text[] text;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !holder.activeSelf)
        {
            holder.SetActive(true);
            return;
        } else if (Input.GetButtonDown("Interact") && holder.activeSelf)
        {
            holder.SetActive(false);
        }
        text[0].text = interact.inventory[0].ToString();
        text[1].text = interact.inventory[1].ToString();
        text[2].text = interact.inventory[2].ToString();
        text[3].text = interact.inventory[3].ToString();
        text[4].text = interact.inventory[4].ToString();
    }

}
