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
    public uint _maxItemsPerRow = 9;
    public uint _maxItemsPerColumn = 2;
    private Vector2 _cursorLocationInInventory = new Vector2(0, 0);
    [HideInInspector]
    public uint currentItemId = 0;

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
    private void Move()
    {
        if(!_showInventory)
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(speed.x * xInput, speed.y * yInput, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.I))
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
        if(_showInventory)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveUpInInventory();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDownInInventory();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeftInInventory();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRightInInventory();
            }
        }
    }

    public void AddObjectToInventory(GameObject itemObject, string texture, string itemName)
    {
        _inventory.AddItem(itemObject, 1); 
        if(_inventory.Items.Count > _stackCounter)
        {
            _showPanel.AddItemImage(texture, itemName);
            _showPanel.SetActiveItem();
            _stackCounter++;
        }
    }

    public void MoveLeftInInventory()
    {
        if (_cursorLocationInInventory.x > 0)
        {
            if(currentItemId % _maxItemsPerRow == 0)
            {
                _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() - 1);
            }
            currentItemId--;
            _cursorLocationInInventory.x--;
            cursor.transform.position += new Vector3(-45, 0, 0);
            _showPanel.SetActiveItem();
        }
    }

    public void MoveRightInInventory()
    {
        if (_cursorLocationInInventory.x < (_maxItemsPerRow - 1))
        {
            if(_inventory.Items.Count > currentItemId + 1)
            {
                currentItemId++;
                if (currentItemId % _maxItemsPerRow == 0)
                {
                    _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() + 1);
                }
                _cursorLocationInInventory.x++;
                cursor.transform.position += new Vector3(45, 0, 0);
                _showPanel.SetActiveItem();
            }
        }
    }

    public void MoveDownInInventory()
    {
        if (_cursorLocationInInventory.y < (_maxItemsPerColumn - 1))
        {
            if(_inventory.Items.Count > currentItemId + _maxItemsPerRow)
            {
                currentItemId += _maxItemsPerRow;
                _cursorLocationInInventory.y++;
                cursor.transform.position += new Vector3(0, -55, 0);
                _showPanel.SetActiveItem();
            }           
        }
    }

    public void MoveUpInInventory()
    {
        if(_cursorLocationInInventory.y > 0)
        {
            currentItemId -= _maxItemsPerRow;
            _cursorLocationInInventory.y--;
            cursor.transform.position += new Vector3(0, 55, 0);
            _showPanel.SetActiveItem();
        }
    }
}
