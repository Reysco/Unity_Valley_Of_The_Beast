using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Tool Action/Harvest")]
public class TilePickUpAction : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        tileMapReadController.cropsManager.PickUp(gridPosition);

        ////removi pois quero que colete apenas a partir do ResourceNode Hit();
        //tileMapReadController.objectsManager.PickUp(gridPosition);

        return true;
    }
}
