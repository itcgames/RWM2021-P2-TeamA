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
    [HideInInspector]
    public int _currentPage = 0;
    [HideInInspector]
    public int _currentlySelectedPage = 0;
    GameObject _activeItem;
    private int _currentIndex = 0;
    public int CurrentIndex { get => _currentIndex;}

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
            _itemPosition.x = -180f;
            _itemPosition.y = 35f;
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
        _itemImages[_currentPage].Add(imgObject);
        if(_currentPage != _currentlySelectedPage)
        {
            _itemImages[_currentPage][_itemImages[_currentPage].Count - 1].SetActive(false);
        }       
    }

    public void SetActiveItem()
    {
        if(_activeItem != null)
        {
            Destroy(_activeItem);
        }
        Vector2 cursorLocation = GetComponentInParent<TestInventoryPlayer>()._cursorLocationInInventory;
        int currentItem = (int)(cursorLocation.x + (cursorLocation.y * GetComponentInParent<TestInventoryPlayer>()._maxItemsPerRow));
        _currentIndex = currentItem;
        Debug.Log("Current item: " + currentItem);
        _activeItem = Instantiate(_itemImages[_currentlySelectedPage][currentItem]);
        _activeItem.transform.position = activeItem.transform.position;
        _activeItem.SetActive(true);
        _activeItem.transform.SetParent(activeItem.transform);
    }

    public void UpdateCurrentlySelectedPage(uint pageNumber)
    {
        for(int i = 0; i < _itemImages[_currentlySelectedPage].Count; i++)
        {
            _itemImages[_currentlySelectedPage][i].SetActive(false);
        }
        _currentlySelectedPage = (int)pageNumber;
        for (int i = 0; i < _itemImages[_currentlySelectedPage].Count; i++)
        {
            _itemImages[_currentlySelectedPage][i].SetActive(true);
        }
    }

    public int GetCurrentlySelectedPage()
    {
        return _currentlySelectedPage;
    }

    public bool IsMorePagesToRight()
    {
        return _currentlySelectedPage < _itemImages.Count - 1;
    }

    public bool IsMorePagesToLeft()
    {
        return _currentlySelectedPage > 0;
    }
    public int NumberOfItemsOnCurrentPage()
    {
        return _itemImages[_currentlySelectedPage].Count;
    }
    public int NumberOfItemsOnPage(int page)
    {
        if (page < 0 || page >= _itemImages.Count) return -1;
        return _itemImages[page].Count;
    }

    public void MoveLeft()
    {
        _currentIndex--;
    }
    public void MoveRight()
    {
        _currentIndex++;
    }
    public void MoveUp()
    {
        _currentIndex -= (int)GetComponentInParent<TestInventoryPlayer>()._maxItemsPerRow;
    }
    public void MoveDown()
    {
        _currentIndex += (int)GetComponentInParent<TestInventoryPlayer>()._maxItemsPerRow;
    }

    public bool IsItemRight()
    {
        return _itemImages[_currentlySelectedPage].Count > _currentIndex + 1;
    }

    public bool IsItemDown()
    {
        return _itemImages[_currentlySelectedPage].Count > _currentIndex + (int)GetComponentInParent<TestInventoryPlayer>()._maxItemsPerRow;
    }

    public bool IsItemUp()
    {
        return _currentIndex - (int)GetComponentInParent<TestInventoryPlayer>()._maxItemsPerRow > 0;
    }

    public int TotalPages()
    {
        return _itemImages.Count;
    }

    public int TotalItems()
    {
        int sum = 0;
        _itemImages.ForEach(x => x.ForEach(y => sum++));
        return sum;
    }
}
