using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObjectsManager : MonoBehaviour
{
    [SerializeField] public PlaceableObjectContainer placeableObjects;
    [SerializeField] public Tilemap targetTilemap;

    private void Start()
    {
        GameManager.instance.GetComponent<PlaceableObjectsReferenceManager>().placeableObjectsManager = this;
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }

            IPersistant persistant = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();
            if(persistant != null)
            {
                string jsonString = persistant.Read();
                placeableObjects.placeableObjects[i].objectState = jsonString;
            }

            placeableObjects.placeableObjects[i].targetObject = null;
        }
    }

    private void VisualizeMap()
    {
        for (int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeableObjects.placeableObjects[i]);
        }
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
        go.transform.parent = transform;

        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionsOnGrid[0]);
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        if(persistant != null)
        {
            persistant.Load(placeableObject.objectState);
        }

        placeableObject.targetObject = go.transform;
    }

    // Modificado para verificar múltiplos espaços no grid
    public bool Check(Vector3Int position, Item item)
    {
        Vector3Int bounds = new Vector3Int(item.size.x, item.size.y, 1); // Define os bounds com base no size do item
        List<Vector3Int> checkPositions = new PlaceableObject(item, position).positionsOnGrid;

        foreach (var checkPosition in checkPositions)
        {
            if (placeableObjects.Get(checkPosition) != null)
            {
                return true; // Se qualquer parte do espaço estiver ocupada, retorna verdadeiro
            }
        }

        return false; // Se todos os espaços estiverem livres, retorna falso
    }

    public void Place(Item item, Vector3Int positionOnGrid)
    {
        if (Check(positionOnGrid, item))
        {
            Debug.Log("Já tem item aqui doido"); // Exibe a mensagem caso o espaço esteja ocupado
            return;
        }

        PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject); // Adiciona o item ao container após garantir que o espaço está livre
    }
}
