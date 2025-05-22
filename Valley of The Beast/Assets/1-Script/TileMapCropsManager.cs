using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapCropsManager : TimeAgent
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] TileBase watered;
    [SerializeField] TileBase dryPlowed;
    [SerializeField] TileBase waterPlowed;

    Tilemap targetTilemap;

    [SerializeField] GameObject cropsSpritePrefabs;

    [SerializeField] CropsContainer container;

    private void Start()
    {
        GameManager.instance.GetComponent<CropManager>().cropsManager = this;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]);
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            container.crops[i].renderer = null;
        }
    }

    public void Tick()
    {
        if (targetTilemap == null) { return; }

        foreach (CropTile cropTile in container.crops)
        {
            if (cropTile.crop == null) { continue; }

            cropTile.damage += 0.02f;

            if (cropTile.damage > 1f)
            {
                cropTile.renderer.sprite = cropTile.crop.witheredSprite; //ativa o sprite da planta morta
                cropTile.crop = null; //retira a semente (o scriptable)
                cropTile.withered = true; //bool para avisar o codigo que a planta morreu
                targetTilemap.SetTile(cropTile.position, plowed); //altera o sprite do terreno para o arado e seco
                continue;
            }

            // Pause o crescimento se a planta tiver completa
            if (cropTile.Complete)
            {
                // Verificar se o dano atingiu 0.8
                if (cropTile.damage >= 0.8f && !cropTile.isDry)
                {
                    targetTilemap.SetTile(cropTile.position, dryPlowed);
                    cropTile.isDry = true;
                }
                continue;
            }

            // Pausa o crescimento se o terreno estiver seco
            if (cropTile.isDry)
            {
                continue;
            }
            // Pausa o crescimento se a planta tiver morta
            if (cropTile.CompleteWithered)
            {
                continue;
            }

            cropTile.growTimer += 1;

            if (cropTile.growTimer >= cropTile.crop.growthStageTimes[cropTile.growStage])
            {
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
                targetTilemap.SetTile(cropTile.position, waterPlowed);

                // Transforma o terreno em seco em cada estágio de crescimento caso a probabilidade seja atingida
                if (cropTile.ProbabilityBecomeDry)
                {
                    targetTilemap.SetTile(cropTile.position, dryPlowed);
                    cropTile.isDry = true;
                }

                cropTile.growStage += 1;
            }
        }
    }

    internal bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if(Check(position) == true) { return; }
        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position)
    {
        if (targetTilemap.GetTile(position) == plowed) //verifica se o terreno está arado e seco (plowed)
        {
            targetTilemap.SetTile(position, seeded);
        }
    }

    public void Water(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.Get(position);

        if (tile == null) { return; }

        if (targetTilemap.GetTile(position) == seeded) //verifica se o terreno já está com a semente (seeded)
        {
            targetTilemap.SetTile(position, watered);
            tile.crop = toSeed;
        }
        else if (targetTilemap.GetTile(position) == dryPlowed) //verifica se o terreno está seco (dryPlowed)
        {
            targetTilemap.SetTile(position, waterPlowed);

            // Despausa o crescimento e reseta o temporizador de morte quando regado
            CropTile cropTile = tile;
            cropTile.isDry = false;
            cropTile.damage = 0;
        }

    }

    public void VisualizeTile(CropTile cropTile)
    {
        targetTilemap.SetTile(cropTile.position, cropTile.crop != null ? seeded : plowed);

        if(cropTile.renderer == null)
        {
            GameObject go = Instantiate(cropsSpritePrefabs, transform);
            Vector3 worldPosition = targetTilemap.CellToWorld(cropTile.position);
            go.transform.position = new Vector3(worldPosition.x + 0.25f, worldPosition.y + 0.25f, worldPosition.z - 0.01f);
            cropTile.renderer = go.GetComponent<SpriteRenderer>();
        }

        bool growing = 
            cropTile.crop != null && 
            cropTile.growTimer >= cropTile.crop.growthStageTimes[0];

        cropTile.renderer.gameObject.SetActive(growing);
        if(growing == true)
        {
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage-1];
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = position;

        VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.Get(gridPosition);
        if(tile == null) { return; }

        if (tile.Complete)
        {
            if (tile.Replacement) // Verifica se a planta é do tipo replacement
            {
                ItemSpawnManager.instance.SpawnItem(
                  targetTilemap.CellToWorld(gridPosition),
                  tile.crop.yield,
                  tile.crop.count
                );

                tile.isDry = true;
                targetTilemap.SetTile(gridPosition, dryPlowed); //quando colher, volta o terreno para o tipo plantado seco
                tile.SetToPreviousStage(); // Se for verdadeira, volta ao estágio anterior de crescimento
            }
            else
            {
                ItemSpawnManager.instance.SpawnItem(
                    targetTilemap.CellToWorld(gridPosition),
                    tile.crop.yield,
                    tile.crop.count
                );

                tile.Harvested();
                VisualizeTile(tile);
            }
        }
        else if (tile.CompleteWithered)
        {
            tile.Harvested();
        }
    }
}
