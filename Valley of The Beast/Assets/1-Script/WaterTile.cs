using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Water Tile")]
public class WaterTile : ToolAction
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

        for(int i=0; i < item.crop.Count; i++)
        {
            tileMapReadController.cropsManager.Water(gridPosition, item.crop[i]);
        }

        AudioManager.instance.Play(onPlowUsed);

        return true;
    }

    //fazer aqui para reduzir % de uso
}
