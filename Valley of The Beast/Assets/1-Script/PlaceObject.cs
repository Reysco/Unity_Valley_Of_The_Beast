using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Place Object")]
public class PlaceObject : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        // Use o tamanho do item para definir os limites
        Vector3Int itemBounds = new Vector3Int(item.size.x, item.size.y, 1);

        // Verifica se há colisão em qualquer parte do espaço que o item ocuparia
        if (tileMapReadController.objectsManager.Check(gridPosition, item))
        {
            Debug.Log("Já tem item aqui doido"); // Exibe a mensagem caso o espaço esteja ocupado
            return false; // Impede a colocação do item se o espaço estiver ocupado
        }

        // Coloca o item no grid considerando os limites
        tileMapReadController.objectsManager.Place(item, gridPosition);
        return true;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
