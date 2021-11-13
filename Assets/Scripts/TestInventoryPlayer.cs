using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInventoryPlayer : MonoBehaviour
{
    public Vector2 speed = new Vector2(20, 20);
    public Text stackAmount;
    public Text itemAmount;
    private Inventory _inventory;
    private int _itemCounter;

    // Start is called before the first frame update
    void Start()
    {
        _inventory = GetComponentInChildren<Inventory>();
        _itemCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * xInput, speed.y * yInput, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    public void UpdateText(GameObject itemObject)
    {
        _inventory.AddItem(itemObject, 1);
        stackAmount.text = "Stack Amount: " + _inventory.Items.Count.ToString();
        _itemCounter++;
        itemAmount.text = "Total Items: " + _itemCounter.ToString();
    }
}
