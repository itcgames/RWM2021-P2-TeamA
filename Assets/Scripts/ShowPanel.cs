using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    public GameObject panelToShow;
    private Vector3 _origanlPosition;


    private void Start()
    {
        _origanlPosition = new Vector3(panelToShow.transform.position.x, panelToShow.transform.position.y, panelToShow.transform.position.z);
    }
    public void PanelShow()
    {
        panelToShow.transform.position -= new Vector3(0, 407);
    }

    public void MovePanelBack()
    {
        panelToShow.transform.position = _origanlPosition;
    }
}
