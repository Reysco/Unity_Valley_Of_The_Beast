using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IconHighlight : MonoBehaviour
{
    public Vector3Int cellPosition;
    private Vector3 targetPosition;
    [SerializeField] private Tilemap targetTilemap;
    private SpriteRenderer spriteRenderer;

    private bool canSelect;
    private bool show;

    [SerializeField] private ToolBarController toolbarController;
    public TileMapReadController tileMapReadController;

    public bool CanSelect
    {
        set
        {
            canSelect = value;
            gameObject.SetActive(canSelect && show);
        }
    }

    public bool Show
    {
        set
        {
            show = value;
            gameObject.SetActive(canSelect && show);
        }
    }

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        // Atualiza a posição do ícone de destaque de acordo com a célula do tilemap
        targetPosition = targetTilemap.CellToWorld(cellPosition);
        transform.position = targetPosition;

        // Obtém o item atual selecionado na toolbar
        Item item = toolbarController.GetItem;

        // Verificar se há algum item na área ocupada considerando os limites do item
        bool hasItem = false;
        if (item != null)
        {
            List<Vector3Int> checkPositions = new PlaceableObject(item, cellPosition).positionsOnGrid;

            //essa gambiarra funciona até onde eu pude perceber
            Vector3Int referenceValue = new Vector3Int(1, 1, 1); //test
            foreach (var checkPosition in checkPositions)
            {
                if (tileMapReadController.objectsManager.Check(checkPosition, item) == tileMapReadController.objectsManager.Check(referenceValue, item))
                {
                    hasItem = false;
                    break;
                }
                else if(tileMapReadController.objectsManager.Check(checkPosition, item) != tileMapReadController.objectsManager.Check(referenceValue, item))
                {
                    hasItem = true;
                    break;
                }
            }
        }

        // Se houver item, mude a cor para vermelho com transparência
        spriteRenderer.color = hasItem
            ? new Color(1f, 0f, 0f, 1f) // vermelho com transparência
            : new Color(1f, 1f, 1f, 0.8f); // branco com transparência
    }

    internal void Set(Sprite icon)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = icon;
    }
}
