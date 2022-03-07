using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayer : MonoBehaviour
{
    [System.Serializable]
    public class ItemPickedUp
    {
        public string deviceUniqueIdentifier;
        public int eventId;
        public string typeOfItem;
    }

    [System.Serializable]
    public class ItemUsed
    {
        public string deviceUniqueIdentifier;
        public int eventId;
        public string typeOfItem;
    }

    public GameObject sword;
    public Vector2 speed = new Vector2(20, 20);
    public Text stackAmount;
    public Text itemAmount;
    public Text bombAmount;
    private int _bombAmount = 0;
    public Text rupeeAmount;
    private int _rupeeAmount = 0;
    public bool useMovement;
    public GameObject cursor;
    [HideInInspector]
    public Inventory _inventory;
    private ShowPanel _showPanel;
    private bool _showInventory;
    private int _stackCounter;
    private Animator _inventoryAnimator;
    private Animator _playerAnimator;
    public GameObject panel;
    public uint _maxItemsPerRow = 9;
    public uint _maxItemsPerColumn = 2;
    [HideInInspector]
    public Vector2 _cursorLocationInInventory = new Vector2(0, 0);
    [HideInInspector]
    public uint currentItemId = 0;
    private Vector2 _direction = new Vector2(0, 0);
    private Player _testPlayer;

    void Start()
    {
        bombAmount.text = "x" + _bombAmount;
        if(rupeeAmount != null)
        {
            rupeeAmount.text = "x" + _rupeeAmount;
        }
        
        _playerAnimator = GetComponent<Animator>();
        _inventoryAnimator = panel.GetComponent<Animator>();
        _showInventory = false;
        _inventoryAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _inventoryAnimator.SetBool("isHidden", _showInventory);
        _showPanel = GetComponentInChildren<ShowPanel>();
        _inventory = GetComponentInChildren<Inventory>();
        _testPlayer = GetComponentInChildren<Player>();
        _stackCounter = 0;
        _direction = Vector2.down;
    }

    public bool IsAttacking()
    {
        return _playerAnimator.GetBool("Attack");
    }

    void OpenInventory()
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

    void UsePotion()
    {
        if (_inventory.Items[_showPanel.CurrentIndex].gameObject.tag == "Potion")
        {
            Debug.Log("trying to use potion");
            if (_testPlayer._health < _testPlayer.maxHealth)
            {
                InventoryItem item = _inventory.Items[_showPanel.CurrentIndex].GetComponent<InventoryItem>();
                if (item.NumberOfItems > 0)
                {
                    int newAmount = (int)item.NumberOfItems - 1;
                    _inventory.Items[_showPanel.CurrentIndex].GetComponent<InventoryItem>().NumberOfItems = (uint)newAmount;
                    PotionScript script = _inventory.Items[_showPanel.CurrentIndex].GetComponent<PotionScript>();
                    _testPlayer.HealPlayerToFull();
                    if (item.NumberOfItems == 0 && script.IsRedPotion)
                    {
                        item.NumberOfItems = 1;
                        script.IsBluePotion = true;
                        script.IsRedPotion = false;
                        _showPanel.SetCurrentItemToBluePotion();

                    }
                    else if (item.NumberOfItems == 0 && script.IsBluePotion)
                    {
                        _showPanel.SetCurrentItemToHidden();
                    }

                    ItemUsed itemUsed = new ItemUsed { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 5, typeOfItem = "Potion" };
                    string jsonData = JsonUtility.ToJson(itemUsed);
                    StartCoroutine(AnalyticsManager.PostMethod(jsonData));
                }
            }

        }
    }

    void MoveInInventory()
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
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            UsePotion();
        }
    }

    void PlaceBomb()
    {
        if (Input.GetKeyDown(KeyCode.B) && _inventory.Items != null)
        {
            List<GameObject> bombs = _inventory.Items.FindAll(x => x.tag == "Bomb");
            bombs = bombs.Where(x => x.GetComponent<InventoryItem>().NumberOfItems > 0).ToList();
            if (bombs.Count > 0)
            {
                bombs[0].GetComponent<InventoryItem>().NumberOfItems--;
                if (bombs[0].GetComponent<InventoryItem>().NumberOfItems == 0)
                {
                    foreach (GameObject item in _inventory.Items)
                    {
                        if (item.GetComponent<InventoryItem>().NumberOfItems == 0)
                        {
                            _showPanel.SetCurrentItemToHidden();
                            break;
                        }
                    }
                }
                GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Bomb"));
                Destroy(bomb.GetComponent<InventoryItem>());
                Destroy(bomb.GetComponent<Item>());
                BombScript script = bomb.AddComponent<BombScript>();
                script.direction = _direction;
                bomb.transform.position = transform.position;
                _bombAmount--;
                bombAmount.text = "x" + _bombAmount;

                ItemUsed itemUsed = new ItemUsed { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 5, typeOfItem = "Bomb" }; ;
                string jsonData = JsonUtility.ToJson(itemUsed);
                StartCoroutine(AnalyticsManager.PostMethod(jsonData));
            }
        }
    }

    private IEnumerator StopAtacking()
    {
        yield return new WaitForSeconds(0.33f);
        _playerAnimator.SetBool("Attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
        if(_showInventory)
        {
            MoveInInventory();
        }
        else
        {
            PlaceBomb();
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

        ItemPickedUp itemPickedUp = new ItemPickedUp { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 4, typeOfItem = itemName };
        string jsonData = JsonUtility.ToJson(itemPickedUp);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
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

    public void AddRupee(int amount)
    {
        if (rupeeAmount != null)
        {
            _rupeeAmount += amount;
            rupeeAmount.text = "x" + _rupeeAmount;
        }
        
    }
}
