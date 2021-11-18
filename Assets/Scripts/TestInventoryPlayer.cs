using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInventoryPlayer : MonoBehaviour
{
    public Vector2 speed = new Vector2(20, 20);
    public Text stackAmount;
    public Text itemAmount;
    public GameObject cursor;
    private Inventory _inventory;
    private ShowPanel _showPanel;
    private bool _showInventory;
    private int _stackCounter;
    private Animator _inventoryAnimator;
    public GameObject panel;
    [SerializeField]
    private uint _maxItemsPerRow = 9;
    [SerializeField]
    private uint _maxItemsPerColumn = 2;
    private Vector2 _cursorLocationInInventory = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        _inventoryAnimator = panel.GetComponent<Animator>();
        _showInventory = false;
        _inventoryAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _inventoryAnimator.SetBool("isHidden", _showInventory);
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
            _inventoryAnimator.SetBool("isHidden", _showInventory);
            if (_showInventory)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void AddObjectToInventory(GameObject itemObject, string texture, string itemName)
    {
        _inventory.AddItem(itemObject, 1); 
        if(_inventory.Items.Count > _stackCounter)
        {
            _showPanel.AddItemImage(texture, itemName);
            _stackCounter++;
        }
    }

    public void MoveLeftInInventory()
    {
        if (_cursorLocationInInventory.x > 0)
        {
            _cursorLocationInInventory.x--;
        }
    }

    public void MoveRightInInventory()
    {
        if (_cursorLocationInInventory.x < (_maxItemsPerRow - 1))
        {
            _cursorLocationInInventory.x++;
        }
    }

    public void MoveDownInInventory()
    {
        if (_cursorLocationInInventory.y < (_maxItemsPerColumn - 1))
        {
            _cursorLocationInInventory.y--;
        }
    }

    public void MoveUpInInventory()
    {
        if(_cursorLocationInInventory.y > 0)
        {
            _cursorLocationInInventory.y--;
        }
    }
}
