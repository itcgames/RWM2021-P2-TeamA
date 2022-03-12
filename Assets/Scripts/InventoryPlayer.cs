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
    private bool _showInventory;
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
    private Vector2 _lastDirectionToAttack = new Vector2(-1, -1);
    int scaleSize = 5;
    public Canvas canvas;
    // Start is called before the first frame update
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
        _inventory = GetComponentInChildren<Inventory>();
        _testPlayer = GetComponentInChildren<Player>();
        _direction = Vector2.down;
        transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    }

    public bool IsAttacking()
    {
        return _playerAnimator.GetBool("Attack");
    }

    private void Move()
    {
        if(!_inventory.IsOpen)
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");
            if(useMovement)
            {
                Vector3 movement = new Vector3(speed.x * xInput, speed.y * yInput, 0);
                movement *= Time.deltaTime;
                transform.Translate(movement);
            }
            if (xInput > 0)
            {
                _direction = Vector2.right;
                transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
                _playerAnimator.SetBool("MoveRight", true);
                _playerAnimator.SetBool("MoveLeft", false);
                _playerAnimator.SetBool("MoveUp", false);
                _playerAnimator.SetBool("MoveDown", false);
            }
            else if (xInput < 0)
            {
                _direction = Vector2.left;
                transform.localScale = new Vector3(-scaleSize, scaleSize, scaleSize);
                _playerAnimator.SetBool("MoveRight", false);
                _playerAnimator.SetBool("MoveLeft", true);
                _playerAnimator.SetBool("MoveUp", false);
                _playerAnimator.SetBool("MoveDown", false);
            }
            if (yInput > 0)
            {
                _direction = Vector2.up;
                _playerAnimator.SetBool("MoveRight", false);
                _playerAnimator.SetBool("MoveLeft", false);
                _playerAnimator.SetBool("MoveUp", true);
                _playerAnimator.SetBool("MoveDown", false);
            }
            else if (yInput < 0)
            {
                _direction = Vector2.down;
                _playerAnimator.SetBool("MoveRight", false);
                _playerAnimator.SetBool("MoveLeft", false);
                _playerAnimator.SetBool("MoveUp", false);
                _playerAnimator.SetBool("MoveDown", true);
            }
        }
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
        if(_inventory.GetCurrentlySelectedObject().gameObject.tag == "Potion")
        {
            _inventory.UseItem();
            ItemUsed itemUsed = new ItemUsed { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 5, typeOfItem = "Potion" };
            string jsonData = JsonUtility.ToJson(itemUsed);
            StartCoroutine(AnalyticsManager.PostMethod(jsonData));
        }
    }

    void MoveInInventory()
    {
        if(_inventory.IsOpen)
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
    }

    void PlaceBomb()
    {
        if (Input.GetKeyDown(KeyCode.B) && _inventory.EquippedItems != null)
        {
            if(_inventory.GetCurrentlySelectedEquippable().gameObject.tag == "Bomb")
            {
                _inventory.UseEquippable();
                GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Bomb"));
                Destroy(bomb.GetComponent<InventoryItem>());
                Destroy(bomb.GetComponent<Item>());
                BombScript script = bomb.AddComponent<BombScript>();
                script.timeToDetonate = 1.5f;
                StartCoroutine(script.DetonateBomb());
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
        Move();
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
            if(_inventory.IsOpen)
            {
                _inventory.CloseInventory();
            }
            else
            {
                _inventory.OpenInventory();
            }
        }
        if(_inventory.IsOpen)
        {
            MoveInInventory();
        }
        else
        {
            PlaceBomb();

            if(Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
            }
        }

        if (IsAttacking())
        {
            Collider2D[] collider2Ds;
            collider2Ds = Physics2D.OverlapCircleAll(transform.position, 2.0f);
            List<Collider2D> colliders = collider2Ds.ToList();
            colliders = colliders.Where(x => x.gameObject.tag == "Enemy").ToList();

            foreach (Collider2D collider in colliders)
            {
                collider.gameObject.GetComponent<Health>().TakeDamage(1.0f, "melee weapon");
            }
        }
    }

    private void Attack()
    {
        bool Attack = _playerAnimator.GetBool("Attack");

        if (!Attack)
        {
            _playerAnimator.SetBool("Attack", true);
            StartCoroutine(StopAtacking());
            if(_testPlayer.IsAtFullHealth())
            {
                GameObject swordclone = CreateSword();

                if(_lastDirectionToAttack != _direction)
                {
                    Destroy(swordclone);
                    CreateSword();
                    _lastDirectionToAttack = _direction;
                }
            }
        }
    }

    GameObject CreateSword()
    {
        GameObject s = Instantiate(sword);
        s.transform.position = transform.position;
        s.transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
        MoveInDirection dir = s.GetComponent<MoveInDirection>();
        dir.direction = _direction;
        dir.speed = 7.5f;

        if (_direction != Vector2.up)
        {
            if (_direction == Vector2.right)
            {
                s.transform.Rotate(new Vector3(0, 0, -90));
            }
            else if (_direction == Vector2.down)
            {
                s.transform.Rotate(new Vector3(0, 0, -180));
            }
            else if (_direction == Vector2.left)
            {
                s.transform.Rotate(new Vector3(0, 0, -270));
            }
        }

        return s;
    }

    public void AddObjectToInventory(GameObject itemObject, string itemName, uint amount)
    {
        if(itemName != "Bomb")
        {
            _inventory.AddItem(itemObject, amount);
        }
        else
        {
            _inventory.AddItemToEquippable(itemObject, amount);
        }
        ItemPickedUp itemPickedUp = new ItemPickedUp { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 4, typeOfItem = itemName };
        string jsonData = JsonUtility.ToJson(itemPickedUp);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
    }

    public void MoveLeftInInventory()
    {
        _inventory.GoToPreviousItem();
    }

    public void MoveRightInInventory()
    {
        _inventory.GoToNextItem();
    }

    public void OpenInventoryNoAnimation()
    {
        _inventory.OpenInventory();
    }

    public void MoveDownInInventory()
    {
        _inventory.GoToItemBelow();
    }

    public void MoveUpInInventory()
    {
        _inventory.GoToItemAbove();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Potion")
        {
            GameObject item = SetUpItem(collision.gameObject);
            _inventory.AddItem(item, 1);
            Destroy(collision.gameObject);
            item.SetActive(false);
            SendAddedItemData(item.GetComponent<InventoryItem>().Name);
        }
        else if(collision.gameObject.tag == "Bomb")
        {
            GameObject item = SetUpItem(collision.gameObject);
            _inventory.AddItemToEquippable(item, 1);
            Destroy(collision.gameObject);
            item.SetActive(false);
            SendAddedItemData(item.GetComponent<InventoryItem>().Name);
        }
        else if(collision.gameObject.tag == "Rupee")
        {
            AddRupee(1);
            Destroy(collision.gameObject);
        }
    }

    GameObject SetUpItem(GameObject item)
    {
        GameObject newItem = Instantiate(item.gameObject);
        if (newItem.GetComponent<SpriteRenderer>() != null)
        {
            Destroy(newItem.GetComponent<SpriteRenderer>());
        }
        if (newItem.GetComponent<Rigidbody2D>() != null)
        {
            Destroy(newItem.GetComponent<Rigidbody2D>());
        }
        if (newItem.GetComponent<BoxCollider2D>() != null)
        {
            Destroy(newItem.GetComponent<BoxCollider2D>());
        }
        newItem.GetComponent<InventoryItem>().canvas = canvas;
        return newItem;
    }

    private void SendAddedItemData(string itemName)
    {
        ItemPickedUp itemPickedUp = new ItemPickedUp { deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 4, typeOfItem = itemName };
        string jsonData = JsonUtility.ToJson(itemPickedUp);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
    }

    public int GetNumberOfItems()
    {
        if(_inventory.Items != null)
            return _inventory.Items.Count;
        return 0;
    }

    public int GetNumberOfEquippables()
    {
        if(_inventory.EquippedItems != null)
            return _inventory.EquippedItems.Count;
        return 0;
    }

    public int GetCurrentIndex()
    {
        if (_inventory.Items != null)
            return _inventory.ActiveItemIndex;
        return -1;
    }
}
