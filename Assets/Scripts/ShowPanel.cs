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
    private List<SpriteRenderer> items;
    public Canvas canvas;

    private void Start()
    {
        _origanlPosition = new Vector3(panelToShow.transform.position.x, panelToShow.transform.position.y, panelToShow.transform.position.z);
        items = new List<SpriteRenderer>();
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
        if(items != null)
        {
            foreach(SpriteRenderer image in items)
            {
                image.transform.position = new Vector3(image.transform.position.x, itemBox.GetComponent<RectTransform>().position.y, 0);
            }
        }
    }


    public void AddItemImage(SpriteRenderer sprite)
    {
        GameObject imgObject = new GameObject("item");
        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(itemBox.transform);
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f);
        Image image = imgObject.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>("Bomb");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(itemBox.transform);
    }
}
