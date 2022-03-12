using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    public bool IsRedPotion;
    [HideInInspector]
    public bool IsBluePotion;

    private void Start()
    {
        IsRedPotion = true;
        IsBluePotion = false;
    }

    InventoryItem _item;
    // Start is called before the first frame update
    void Awake()
    {
        _item = GetComponent<InventoryItem>();
        _item.useFunction += HealPlayer;
    }

    bool HealPlayer()
    {
        Debug.Log("Healing player");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Player script = player.GetComponent<Player>();
        return script.HealPlayerToFull();
    }
}
