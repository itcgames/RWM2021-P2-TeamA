using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInventoryPlayer : MonoBehaviour
{
    public Vector2 speed = new Vector2(20, 20);
    public Text stackAmount;
    public Text itemAmount;
    public Text bombAmount;
    private int _bombAmount = 0;

    public GameObject cursor;
    [HideInInspector]
    public Inventory _inventory;
    private ShowPanel _showPanel;
    private bool _showInventory;
    private int _stackCounter;
    private Animator _inventoryAnimator;
    public GameObject panel;
    public uint _maxItemsPerRow = 9;
    public uint _maxItemsPerColumn = 2;
    [HideInInspector]
    public Vector2 _cursorLocationInInventory = new Vector2(0, 0);
    [HideInInspector]
    public uint currentItemId = 0;
    private Vector2 _direction = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
        bombAmount.text = "x" + _bombAmount;
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
            if (xInput > 0) _direction = Vector2.right;
            else if (xInput < 0) _direction = Vector2.left;
            if (yInput > 0) _direction = Vector2.up;
            else if (yInput < 0) _direction = Vector2.down;
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
        else
        {
            if(Input.GetKeyDown(KeyCode.B) && _inventory.Items != null)
            {
                List<GameObject> bombs = _inventory.Items.FindAll(x => x.tag == "Bomb");
                if(bombs.Count > 0)
                {
                    bombs[0].GetComponent<InventoryItem>().NumberOfItems--;
                    if(bombs[0].GetComponent<InventoryItem>().NumberOfItems == 0)
                    {
                        _inventory.Items.RemoveAll(x => x.GetComponent<InventoryItem>().NumberOfItems == 0);
                    }
                    GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Bomb"));
                    Destroy(bomb.GetComponent<InventoryItem>());
                    Destroy(bomb.GetComponent<TestItem>());
                    BombScript script = bomb.AddComponent<BombScript>();
                    script.direction = _direction;
                    bomb.transform.position = transform.position;
                    _bombAmount--;
                    bombAmount.text = "x" + _bombAmount;
                }
            }
        }
    }

    public void AddObjectToInventory(GameObject itemObject, string texture, string itemName, uint amount)
    {
        _inventory.AddItem(itemObject, amount); 
        if(_inventory.Items.Count > _stackCounter)
        {
            _showPanel.AddItemImage(texture, itemName);
            _showPanel.SetActiveItem();
            _stackCounter++;
        }
    }

    public void MoveLeftInInventory()
    {
        if (_showPanel.CurrentIndex > 0 && _showPanel.CurrentIndex < _maxItemsPerRow ||
            _showPanel.CurrentIndex > _maxItemsPerRow)
        {
            _showPanel.MoveLeft();
            _cursorLocationInInventory.x--;
            cursor.transform.position += new Vector3(-45, 0, 0);
            _showPanel.SetActiveItem();
            return;
        }
        if(_showPanel.CurrentIndex == 0 && _showPanel.IsMorePagesToLeft())
        {
            _cursorLocationInInventory = new Vector2(_maxItemsPerRow - 1, 0);
            cursor.transform.position += new Vector3(45 * (_maxItemsPerRow - 1), 0, 0);
            _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() - 1);
            _showPanel.SetActiveItem();
        }
        else if(_showPanel.CurrentIndex == _maxItemsPerRow && _showPanel.IsMorePagesToLeft())
        {
            _cursorLocationInInventory = new Vector2(_maxItemsPerRow - 1, 1);
            cursor.transform.position += new Vector3(45 * (_maxItemsPerRow - 1), 0, 0);
            _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() - 1);
            _showPanel.SetActiveItem();
        }
    }

    public void MoveRightInInventory()
    {
        if(_showPanel.CurrentIndex < (_maxItemsPerRow - 1) || (_cursorLocationInInventory.y == 1 && _showPanel.CurrentIndex < _maxItemsPerRow * _maxItemsPerColumn - 1))
        {
            if(_showPanel.IsItemRight())
            {
                _showPanel.MoveRight();
                _cursorLocationInInventory.x++;
                cursor.transform.position += new Vector3(45, 0, 0);
                _showPanel.SetActiveItem();
                return;
            }
        }

        if (_cursorLocationInInventory.y == 0 && _showPanel.CurrentIndex == _maxItemsPerRow - 1)
        {
            if (_showPanel.IsMorePagesToRight())
            {
                _cursorLocationInInventory = new Vector2(0, 0);
                cursor.transform.position -= new Vector3(45 * (_maxItemsPerRow - 1), 0, 0);
                _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() + 1);
                _showPanel.SetActiveItem();
            }
        }
        else if(_cursorLocationInInventory.y == 1 && _showPanel.CurrentIndex == _maxItemsPerRow * _maxItemsPerColumn - 1)
        {
            if (_showPanel.IsMorePagesToRight())
            {
                if(_showPanel.NumberOfItemsOnPage(_showPanel.GetCurrentlySelectedPage() + 1) > _maxItemsPerRow)
                {
                    _cursorLocationInInventory = new Vector2(0, 1);
                    cursor.transform.position -= new Vector3(45 * (_maxItemsPerRow - 1), 0, 0);
                }
                else
                {
                    _cursorLocationInInventory = new Vector2(0, 0);
                    cursor.transform.position -= new Vector3(45 * (_maxItemsPerRow - 1), -55, 0);
                }
                               
                _showPanel.UpdateCurrentlySelectedPage((uint)_showPanel.GetCurrentlySelectedPage() + 1);
                _showPanel.SetActiveItem();
            }
        }
    }

    public void MoveDownInInventory()
    {
        if (_cursorLocationInInventory.y < (_maxItemsPerColumn - 1))
        {
            if(_showPanel.IsItemDown())
            {
                _showPanel.MoveDown();
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
            _showPanel.MoveUp();
            _cursorLocationInInventory.y--;
            cursor.transform.position += new Vector3(0, 55, 0);
            _showPanel.SetActiveItem();
        }
    }

    public uint GetAmountOfItemAtPosition(int index)
    {
        return _inventory.Items[index].GetComponent<InventoryItem>().NumberOfItems;
    }

    public void AddBomb(int amount)
    {
        _bombAmount += amount;
        bombAmount.text = "x" + _bombAmount;
    }
}
