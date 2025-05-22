using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemConvertorData
{
    public ItemSlot itemSlot;
    public int timer;

    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
    }

}

[RequireComponent(typeof(TimeAgent))]
public class ItemConvertorInteract : Interactable, IPersistant
{
    [SerializeField] Item convertableItem;
    [SerializeField] Item producedItem;
    [SerializeField] int producedItemCount = 1;

    [SerializeField] int timeToProcess = 5;

    ItemConvertorData data;

    Animator anim;

    [SerializeField] GameObject canvas;
    [SerializeField] Image iconImage;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;

        if(data == null)
        {
            data = new ItemConvertorData();
        }
        anim = GetComponent<Animator>();
        Animate();
    }

    private void ItemConvertProcess()
    {
        if (data.itemSlot == null) { return; }
        if (data.timer > 0)
        {
            data.timer -= 1;
            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    private void Update()
    {
        ItemHighlight();
    }

    public override void Interact(Character character)
    {
        if(data.itemSlot.item == null)
        {
            if (GameManager.instance.dragAndDropController.Check(convertableItem))
            {
                StartItemProcessing(GameManager.instance.dragAndDropController.itemSlot);
                return;
            }

            ToolBarController toolbarController = character.GetComponent<ToolBarController>();
            if(toolbarController == null) { return; }

            ItemSlot itemSlot = toolbarController.GetItemSlot;

            if(itemSlot.item == convertableItem)
            {
                StartItemProcessing(itemSlot);
                return;
            }
        }

        if(data.itemSlot.item != null && data.timer <= 0)
        {
            GameManager.instance.inventoryContainer.Add(data.itemSlot.item, data.itemSlot.count);
            data.itemSlot.Clear();
        }
    }

    private void StartItemProcessing(ItemSlot toProcess)
    {
        data.itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        data.itemSlot.count = 1;

        if (toProcess.item.stackable)
        {
            toProcess.count -= 1;
            if(toProcess.count < 0)
            {
                toProcess.Clear();
            }
        }
        else
        {
            toProcess.Clear();
        }

        data.timer = timeToProcess;
        Animate();
    }

    private void ItemHighlight()
    {
        if (data.itemSlot.item == null || data.timer > 0f)
        {
            iconImage.sprite = null;
            canvas.SetActive(false);
        }
        else
        {
            iconImage.sprite = producedItem.icon;
            canvas.SetActive(true);
        }
    }

    private void Animate()
    {
        anim.SetBool("Working", data.timer > 0f);
    }

    private void CompleteItemConversion()
    {
        Animate();
        data.itemSlot.Clear();
        data.itemSlot.Set(producedItem, producedItemCount);
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConvertorData>(jsonString);
    }
}
