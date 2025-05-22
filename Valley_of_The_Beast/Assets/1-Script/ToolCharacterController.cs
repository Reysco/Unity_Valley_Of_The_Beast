using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class ToolCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rgbd2d;
    ToolBarController toolbarController;
    Animator animator;
    [SerializeField] float offsetDistance = 1f; //distancia maxima do player para interagir
    [SerializeField] float sizeOfInteractableArea = 1.2f; //tamanho da area de interação do player (para quebrar arvore por exemplo)
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f; //até onde é a area de interação do mouse para aparecer o marcador e interagir (tipo plantar)
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHighlight;
    public ListOfAnimationOfWeaponsAndTools animationWeaponTool;

    Vector3Int selectedTilePosition;
    bool selectable;

    LineRenderer maxDistanceRenderer; //para ver onde é a area de interação
    LineRenderer interactableAreaRenderer; //para ver onde é a area de interação

    int currentWeaponNo; //test


    public float originSpeed;
    public float zeroSpeed;

    public bool canAction;


    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolBarController>();
        animator = GetComponent<Animator>();

        // Inicializar os LineRenderers
        maxDistanceRenderer = CreateLineRenderer(Color.red);
        interactableAreaRenderer = CreateLineRenderer(Color.blue);

        // Inicializar as variaveis de speed do player para desativar durante uso de ferramentas
        originSpeed = character.speed;
        zeroSpeed = 0;

        // Variavel para desativar clique do mouse quando player estiver executando açao
        canAction = true;

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canAction == true)//se eu apertar botao esquerdo enquanto eu puder fazer a açao, usarei a ferramenta/arma
        {
            WeaponAction();

            if (UseToolWorld() == true)
            {
                return;
            }
            UseToolGrid();
        }

        SelectTile();
        CanSelectCheck();
        Marker();
        UpdateRangeVisuals(); //para ver onde é a area de interação
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
        iconHighlight.cellPosition = selectedTilePosition;
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;
    }

    private void WeaponAction()
    {
        Item item = toolbarController.GetItem;
        if (item == null) { return ; }
        if (item.isWeapon == false) { return ; }

        //attackController.Attack(item.damage);

        if (item.isWeapon == true && animator.GetInteger("tool") == 0)
        {
            SwordAnimationAndAction(item);
        }
    }

    private bool UseToolWorld()
    {
        //verificar se o personagem está perto e está virado para o lado correto
        Vector2 position = rgbd2d.position + character.lastMotionVector * offsetDistance;

        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if (item.onAction == null) { return false; }

        if(item.isPickaxe == true && animator.GetInteger("tool") == 0)
        {
            AnimationAndAction(item);
        }

        if (item.isAxe == true && animator.GetInteger("tool") == 0)
        {
            AnimationAndAction(item);
        }

        bool complete = item.onAction.OnApply(position);

        if (complete == true)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer); //aqui usa o item, a semenete
            }
        }

        return complete;
    }

    private void UseToolGrid() //aqui implemento para adicionar o terreno arado
    {
        if (selectable)
        {
            Item item = toolbarController.GetItem;
            if (item == null)
            {
                PickUpTile(); //se não tiver nada na mão, colhe a plantação
                return;
            }
            if (item.onTileMapAction == null) { return; }


            //serve para a animação do Red_WaterCan
            if (item.isWaterCan == true && animator.GetInteger("tool") == 0)
            {
                WaterAnimationAndAction(item);
            }


            if (item.isHoe == true && animator.GetInteger("tool") == 0)
            {
                AnimationAndAction(item);
            }








            bool complete = item.onTileMapAction.OnApplyToTileMap(
                selectedTilePosition,
                tileMapReadController,
                item
                );

            if (complete)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer); //aqui usa o item, a semente por ex
                }
            }
        }
    }

    private void PickUpTile()
    {
        if (onTilePickUp == null) { return; }

        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }

    //desenha círculo em volta do player para saber onde é a area de interação

    // Método para criar e configurar um LineRenderer
    private LineRenderer CreateLineRenderer(Color color)
    {
        GameObject lineObj = new GameObject("LineRenderer");
        lineObj.transform.parent = transform;

        // Ajustar a posição para (0, 0, 0.1)
        lineObj.transform.localPosition = new Vector3(0, 0, 0.1f);

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0; // Inicialmente não desenha nada
        lineRenderer.useWorldSpace = false; // Para que o LineRenderer use o espaço local

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = color;

        return lineRenderer;
    }

    // Método para atualizar a visualização dos ranges
    private void UpdateRangeVisuals()
    {
        DrawCircle(maxDistanceRenderer, Vector3.zero, maxDistance);
        Vector2 interactPosition = character.lastMotionVector * offsetDistance;
        DrawCircle(interactableAreaRenderer, interactPosition, sizeOfInteractableArea);
    }

    // Método para desenhar um círculo usando LineRenderer
    private void DrawCircle(LineRenderer lineRenderer, Vector3 offset, float radius)
    {
        int segments = 50;
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = 0.1f; // Ajuste no eixo Z
            lineRenderer.SetPosition(i, new Vector3(x, y, z) + offset);
            angle += (360f / segments);
        }
    }


    private void AnimationAndAction(Item item)
    {
        animator.SetInteger("tool", 1); //garante que a animação vai acontecer somente junto com a animaçao da ferramenta
        animator.SetTrigger("act"); //animação de usar
        animationWeaponTool.animator.SetInteger("tool", item.id);
        character.speed = zeroSpeed;
        canAction = false;
    }

    private void WaterAnimationAndAction(Item item)
    {
        animator.SetInteger("tool", 1); //garante que a animação vai acontecer somente junto com a animaçao da ferramenta
        animator.SetTrigger("waterCanAct"); //animação de usar
        animationWeaponTool.animator.SetInteger("tool", item.id);
        character.speed = zeroSpeed;
        canAction = false;
    }

    private void SwordAnimationAndAction(Item item)
    {
        animator.SetInteger("tool", 1); //garante que a animação vai acontecer somente junto com a animaçao da ferramenta
        animator.SetTrigger("swordAct"); //animação de usar
        animationWeaponTool.animator.SetInteger("tool", item.id);
        character.speed = zeroSpeed;
        canAction = false;
    }


    //para resetar a animação de tool
    public void ResetToolWeaponAnimation()
    {
        animator.SetInteger("tool", 0);
        animationWeaponTool.animator.SetInteger("tool", 0);
    }

    //para resetar a speed do player
    public void ResetPlayerSpeedAndAction()
    {
        character.speed = originSpeed;
        canAction = true;
    }
}