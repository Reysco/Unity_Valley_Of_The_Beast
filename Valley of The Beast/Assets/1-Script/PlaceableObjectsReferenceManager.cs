using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectsReferenceManager : MonoBehaviour
{
    public PlaceableObjectsManager placeableObjectsManager;

    public void Place(Item item, Vector3Int pos)
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogWarning("No placeableObjects reference detected");
            return;
        }

        placeableObjectsManager.Place(item, pos);
    }

    public bool Check(Vector3Int pos, Item item)
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogWarning("No placeableObjects reference detected");
            return false;
        }
        return placeableObjectsManager.Check(pos, item);
    }
}
