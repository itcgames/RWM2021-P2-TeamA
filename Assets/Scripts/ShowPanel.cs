using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    public GameObject panelToShow;
    public Image itemBox;
    public Image activeItem;
    private Vector3 _origanlPosition;
    private Vector2 _itemPosition;
    public Canvas canvas;
    private int _itemCount = 0;

    private void Start()
    {
        _origanlPosition = new Vector3(panelToShow.transform.position.x, panelToShow.transform.position.y, panelToShow.transform.position.z);
        _itemPosition = new Vector2(-180f, 35f);
    }
    public void PanelShow()
    {
        panelToShow.transform.position -= new Vector3(0, 407);
    }

    public void MovePanelBack()
    {
        panelToShow.transform.position = _origanlPosition;
    }

    private void Update()
    {

    }


    public void AddItemImage(string texture, string itemName)
    {
        _itemCount++;
        if (_itemCount > 18) return;
        GameObject imgObject = new GameObject(itemName);
        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(itemBox.transform);
        trans.localScale = Vector3.one;
        trans.anchoredPosition = _itemPosition;
        _itemPosition.x += 45;
        if(_itemCount == 9)
        {
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
