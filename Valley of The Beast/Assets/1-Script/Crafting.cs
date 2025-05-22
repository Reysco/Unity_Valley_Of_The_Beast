using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;

    public void Craft(CraftingRecipe recipe)
    {
        if(inventory.CheckFreeSpace() == false)
        {
            Debug.Log("Sem espaço suficiente no inventário");

            return;
        }

        for(int i =0; i<recipe.elements.Count; i++) //aqui é para fazer se não tiver os itens, ficar vermelho as letras
        {
            if (inventory.CheckItem(recipe.elements[i]) == false)
            { 
                Debug.Log("Está faltando itens necessário em seu inventario para o craft");
                return;
            }

        }

        for(int i =0; i < recipe.elements.Count; i++) //ao terminar de clicar, ir para o mouse
        {
            inventory.Remove(recipe.elements[i].item, recipe.elements[i].count);
        }

        inventory.Add(recipe.output.item, recipe.output.count); // aqui adiciona direto no inventário

    }
}
