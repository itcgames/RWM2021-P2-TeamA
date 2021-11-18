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
    public uint NumberPerRow;
    private uint _numberPerRow = 0;
    public uint NumberOfColumns;
    private uint _numberOfColumns = 0;

    private void Start()
    {
        _itemPosition = new Vector2(-180f, 35f);
    }

    public void AddItemImage(string texture, string itemName)
    {
        if (_numberOfColumns >= NumberOfColumns && _numberPerRow >= NumberPerRow * NumberOfColumns)
        {
            return;
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
    }
}
