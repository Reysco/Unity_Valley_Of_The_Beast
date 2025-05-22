using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public int id;
    public bool stackable; //verificar se pode stackar
    public Sprite icon;
    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;

    //public Crop crop;
    public List<Crop> crop; // Aqui colocarei nos regadores todas as sementes que podem ser regadas

    public bool iconHighlight;
    public GameObject itemPrefab;

    // Adicionando o tamanho do item em termos de células
    public Vector2Int size = new Vector2Int(1, 1); // Tamanho padrão de 1x1 célula

    public int damage = 10;

    public bool isPickaxe;
    public bool isAxe;
    public bool isHoe;
    public bool isWaterCan;
    public bool isWeapon;


}