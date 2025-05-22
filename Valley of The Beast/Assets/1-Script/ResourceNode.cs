using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] float spread = 0.7f;

    [SerializeField] Item item;
    [SerializeField] int itemCountInOneDrop = 1;
    [SerializeField] int dropCount = 5;
    [SerializeField] ResourceNodeType nodeType;

    // Refer�ncia ao PlaceableObjectsManager
    private PlaceableObjectsManager placeableObjectsManager;

    private void Awake()
    {
        // Encontre o PlaceableObjectsManager na cena
        placeableObjectsManager = FindObjectOfType<PlaceableObjectsManager>(); //essa forma faz com que sempre que iniciar o objeto, ele procure o PlaceableObjectsManager na cena

        if (placeableObjectsManager == null)
        {
            Debug.LogError("PlaceableObjectsManager n�o encontrado na cena!");
        }
    }

    public override void Hit()
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogError("PlaceableObjectsManager n�o est� atribu�do!");
            return;
        }

        // Obtenha a posi��o no grid
        Vector3Int gridPosition = GetGridPosition();

        // Verifique se h� um item no container e remova-o
        PlaceableObject placedObject = placeableObjectsManager.placeableObjects.Get(gridPosition);
        if (placedObject != null)
        {
            // Remova o item do container
            placeableObjectsManager.placeableObjects.Remove(placedObject);
            //Debug.Log("Removi o item do container");
        }

        // Gere os itens ca�dos
        while (dropCount > 0)
        {
            dropCount -= 1;

            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;

            ItemSpawnManager.instance.SpawnItem(position, item, itemCountInOneDrop);
        }

        // Destrua o ResourceNode
        Destroy(gameObject);
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }

    // M�todo para obter a posi��o do grid
    private Vector3Int GetGridPosition()
    {
        return placeableObjectsManager.targetTilemap.WorldToCell(transform.position);
    }
}
