using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;

    public GameObject backGroundContainerPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && backGroundContainerPanel.activeSelf == false) //se apertar o I ao msm tempo que não tem baú aberto
        {
            if(panel.activeInHierarchy == false)
            {
                Open();
                statusPanel.SetActive(true);
            }
            else
            {
                Close();
                statusPanel.SetActive(false);
            }
        }
    }

    public void Open()
    {
        panel.SetActive(true);
        toolbarPanel.SetActive(false);
    }

    public void Close()
    {
        panel.SetActive(false);
        toolbarPanel.SetActive(true);
    }
}
