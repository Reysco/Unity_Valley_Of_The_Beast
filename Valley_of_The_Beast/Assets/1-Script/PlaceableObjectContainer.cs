using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public Item placedItem;
    public Transform targetObject;
    public List<Vector3Int> positionsOnGrid; // Lista de todas as posições ocupadas pelo item

    /// <summary>
    /// serialized JSON string which contains the state of the object
    /// </summary>
    public string objectState;

    public PlaceableObject(Item item, Vector3Int pos)
    {
        placedItem = item;
        Vector3Int bounds = new Vector3Int(item.size.x, item.size.y, 1); // Usando o size do item para definir os bounds
        positionsOnGrid = GeneratePositions(pos, bounds);
    }

    private List<Vector3Int> GeneratePositions(Vector3Int startPosition, Vector3Int bounds)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        for (int x = 0; x < bounds.x; x++)
        {
            for (int y = 0; y < bounds.y; y++)
            {
                positions.Add(startPosition + new Vector3Int(x, y, 0));
            }
        }

        return positions;
    }
}

[CreateAssetMenu(menuName = "Data/Placeable Objects Container")]
public class PlaceableObjectContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    internal PlaceableObject Get(Vector3Int position)
    {
        return placeableObjects.Find(x => x.positionsOnGrid.Contains(position));
    }

    internal void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    }
}