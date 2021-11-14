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
    private ShowPanel _showPanel;
    private bool _showInventory;
    private int _stackCounter;

    // Start is called before the first frame update
    void Start()
    {
        _showInventory = false;
        _showPanel = GetComponentInChildren<ShowPanel>();
        _inventory = GetComponentInChildren<Inventory>();
        _stackCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * xInput, speed.y * yInput, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);

        if(Input.GetKeyDown(KeyCode.I))
        {
            _showInventory = !_showInventory;
            if (_showInventory)
            {
                _showPanel.PanelShow();
                Time.timeScale = 0;
            }
            else
            {
                _showPanel.MovePanelBack();
                Time.timeScale = 1;
            }
        }
    }

    public void UpdateText(GameObject itemObject, SpriteRenderer image)
    {
        _inventory.AddItem(itemObject, 1); 
        if(_inventory.Items.Count > _stackCounter)
        {
            _showPanel.AddItemImage(image);
            _stackCounter++;
        }
    }
}
