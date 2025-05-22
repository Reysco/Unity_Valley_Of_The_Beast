using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;
    public bool isDirty;

    internal void Init()
    {
        slots = new List<ItemSlot>();
        for(int i = 0; i < 30; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    public void Add(Item item, int count = 1)
    {
        isDirty= true;

        if(item.stackable == true)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);
            if(itemSlot != null)
            {
                itemSlot.count += count;
            }
            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if(itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.count = count;
                }
            }
        }
        else
        {
            //adicionar não stackavel item no nosso inventario
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if(itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
    }

    public void Remove(Item itemToRemove, int count = 1) //posso implementar algo do tipo para itens usaveis
    {
        isDirty = true;

        if (itemToRemove.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
            if(itemSlot == null) { return; }

            itemSlot.count -= count;
            if(itemSlot.count <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            while (count > 0)
            {
                count -= 1;

                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
                if(itemSlot == null) { return; }

                itemSlot.Clear();
            }
        }
    }

    internal bool CheckFreeSpace() //verificar se tem espaço disponivel no invantario
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                return true;
            }
        }

        return false;
    }

    internal bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);

        /* outra forma de escrever isso de cima
        ItemSlot itemSlot = null;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == checkingItem.item)
            {
                itemSlot = slots[i];
                break;
            }
        }*/


        if (itemSlot == null) { return false; }

        if(checkingItem.item.stackable) {  return itemSlot.count > checkingItem.count; }

        return true;
    }

}
