using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 10; // tamanho total da barra de ferramentas
    int selectedTool;

    public Action<int> onChange;
    [SerializeField] IconHighlight iconHighlight;

    public ItemSlot GetItemSlot
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool];
        }
    }

    public Item GetItem
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool].item;
        }
    }

    private void Start()
    {
        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;

        if (delta != 0)
        {
            if (delta > 0)
            {
                selectedTool += 1;
                selectedTool = (selectedTool >= toolbarSize ? 0 : selectedTool);
            }
            else
            {
                selectedTool -= 1;
                selectedTool = (selectedTool < 0 ? toolbarSize - 1 : selectedTool);
            }
            onChange?.Invoke(selectedTool);
        }

        // Verificar se uma tecla num�rica de 1 a 0 foi pressionada
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || (i == 9 && Input.GetKeyDown(KeyCode.Alpha0)))
            {
                selectedTool = i;
                onChange?.Invoke(selectedTool);
                break;
            }
        }
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }

    public void UpdateHighlightIcon(int id = 0)
    {
        Item item = GetItem;
        if (item == null)
        {
            iconHighlight.Show = false;
            return;
        }

        iconHighlight.Show = item.iconHighlight;
        if (item.iconHighlight)
        {
            iconHighlight.Set(item.icon);
        }
    }
}