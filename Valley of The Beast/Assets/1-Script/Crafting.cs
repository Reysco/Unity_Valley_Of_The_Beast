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
            Debug.Log("Sem espa�o suficiente no invent�rio");

            return;
        }

        for(int i =0; i<recipe.elements.Count; i++) //aqui � para fazer se n�o tiver os itens, ficar vermelho as letras
        {
            if (inventory.CheckItem(recipe.elements[i]) == false)
            { 
                Debug.Log("Est� faltando itens necess�rio em seu inventario para o craft");
                return;
            }

        }

        for(int i =0; i < recipe.elements.Count; i++) //ao terminar de clicar, ir para o mouse
        {
            inventory.Remove(recipe.elements[i].item, recipe.elements[i].count);
        }

        inventory.Add(recipe.output.item, recipe.output.count); // aqui adiciona direto no invent�rio

    }
}
