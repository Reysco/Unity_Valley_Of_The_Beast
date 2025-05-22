using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Seed Tile")]
public class SeedTile : ToolAction
{
    [SerializeField] AudioClip onPlowUsed;

    public override bool OnApplyToTileMap(Vector3Int gridPosition, 
        TileMapReadController tileMapReadController, 
        Item item)
    {
        if (tileMapReadController.cropsManager.Check(gridPosition) == false)
        {
            return false;
        }

        tileMapReadController.cropsManager.Seed(gridPosition);

        AudioManager.instance.Play(onPlowUsed);

        return true;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
