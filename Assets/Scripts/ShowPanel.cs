using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    public GameObject panelToShow;
    public Image itemBox;
    public Image activeItem;
    private Vector2 _itemPosition;
    public Canvas canvas;
    private int _itemCount = 0;
    private  uint NumberPerRow;
    private uint _numberPerRow = 0;
    private uint NumberOfColumns;
    private uint _numberOfColumns = 0;
    private List<List<GameObject>> _itemImages;
    private int _currentPage = 0;
    private int _currentlySelectedPage = 0;
    GameObject _activeItem;

    private void Start()
    {
        TestInventoryPlayer script = GetComponentInParent<TestInventoryPlayer>();
        NumberPerRow = script._maxItemsPerRow;
        NumberOfColumns = script._maxItemsPerColumn;
        _itemPosition = new Vector2(-180f, 35f);
        _itemImages = new List<List<GameObject>>();
        _itemImages.Add(new List<GameObject>());
    }

    public void AddItemImage(string texture, string itemName)
    {
        if (_numberOfColumns >= NumberOfColumns && _numberPerRow >= NumberPerRow * NumberOfColumns)
        {
            _itemImages.Add(new List<GameObject>());
            _currentPage++;
            _numberPerRow = 0;
            _numberOfColumns = 0;
            _itemPosition.x = 180f;
            _itemPosition.y -= 55f;
        }
        _itemCount++;
        _numberPerRow++;
        if (_numberOfColumns == 0) _numberOfColumns++;
        if (_itemCount > GetComponentInParent<Inventory>().MaxStackAmount) return;
        GameObject imgObject = new GameObject(itemName);
        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(itemBox.transform);
        trans.localScale = Vector3.one;
        trans.anchoredPosition = _itemPosition;
        _itemPosition.x += 45;
        if(_numberPerRow == NumberPerRow && _numberOfColumns < NumberOfColumns)
        {
            _numberOfColumns++;
            _itemPosition.x = -180f;
            _itemPosition.y -= 55f;
        }
        trans.sizeDelta = new Vector2(30, 30);
        Image image = imgObject.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>(texture);
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(itemBox.transform);
        _itemImages[_currentPage].Add(Instantiate(imgObject));
        _itemImages[_currentPage][_itemImages[_currentPage].Count - 1].SetActive(false);
    }

    public void SetActiveItem()
    {
        if(_activeItem != null)
        {
            _activeItem.SetActive(false);
        }
        int currentItem = (int)GetComponentInParent<TestInventoryPlayer>().currentItemId;
        _activeItem = _itemImages[_currentlySelectedPage][currentItem];
        _activeItem.transform.position = activeItem.transform.position;
        _activeItem.SetActive(true);
        _activeItem.transform.SetParent(activeItem.transform);
    }

    public void UpdateCurrentlySelectedPage(uint pageNumber)
    {
        foreach(GameObject obj in _itemImages[_currentlySelectedPage])
        {
            obj.SetActive(false);
        }
        _currentlySelectedPage = (int)pageNumber;
        foreach (GameObject obj in _itemImages[_currentlySelectedPage])
        {
            obj.SetActive(true);
        }
    }

    public int GetCurrentlySelectedPage()
    {
        return _currentlySelectedPage;
    }
}
