using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[Serializable]
public class CropTile
{
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer renderer;
    public float damage;
    public Vector3Int position;

    public bool withered = false;
    public bool isDry = false; // Indica se o terreno está seco

    public float dryProbability;
    public bool replacement;

    public bool Complete
    {
        get
        {
            if (crop == null) { return false; }
            return growTimer >= crop.timeToGrow;
        }
    }

    public bool CompleteWithered
    {
        get
        {
            return withered;
        }
    }

    public bool ProbabilityBecomeDry
    {
        get
        {
            dryProbability = crop.dryProbability;
            return UnityEngine.Random.value <= dryProbability;
        }
    }

    public bool Replacement
    {
        get
        {
            replacement = crop.replacement;
            return replacement;
        }
    }

    internal void Harvested()
    {
        growTimer = 0;
        growStage = 0;
        crop = null;
        renderer.gameObject.SetActive(false);
        damage = 0;
        withered = false;
        isDry = false; // Reseta o estado do terreno ao colher
    }

    internal void SetToPreviousStage() //voltar estágio anterior
    {
        if (growStage > 0)
        {
            growStage--; // Volta para o estágio de crescimento anterior
            growTimer = crop.growthStageTimes[growStage] - 1; // Ajusta o temporizador de crescimento
            renderer.sprite = crop.sprites[growStage - 1]; // Atualiza o sprite para o estágio anterior
        }
    }
}

public class CropManager : MonoBehaviour
{
    public TileMapCropsManager cropsManager;

    public void PickUp(Vector3Int position)
    {
        if(cropsManager == null) 
        {
            Debug.LogWarning("Sem tilemap crops manager referenciados no crops manager");
            return; 
        }

        cropsManager.PickUp(position);
    }

    public bool Check(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("Sem tilemap crops manager referenciados no crops manager");
            return false;
        }

        return cropsManager.Check(position);
    }

    public void Seed(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("Sem tilemap crops manager referenciados no crops manager");
            return;
        }

        cropsManager.Seed(position);
    }

    public void Water(Vector3Int position, Crop toSeed)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("Sem tilemap crops manager referenciados no crops manager");
            return;
        }

        cropsManager.Water(position, toSeed);
    }

    public void Plow(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("Sem tilemap crops manager referenciados no crops manager");
            return;
        }

        cropsManager.Plow(position);
    }
}