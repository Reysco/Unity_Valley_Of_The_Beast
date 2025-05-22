using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Crop")]
public class Crop : ScriptableObject
{
    public int timeToGrow = 10;
    public Item yield;
    public int count = 1;

    public List<Sprite> sprites;
    public Sprite witheredSprite;
    public List<int> growthStageTimes;

    [Header("Probabilidade de seca a cada estágio 0 - 1")]
    public float dryProbability;

    [Header("Marcar se puder ser colhida sem necessidade de replantar")]
    public bool replacement;


    [Header("Marcar quais estações ela poderá crescer FALTA IMPLEMENTAR")]
    public bool isGreenhouse;
    public bool summer;
    public bool autumn;
    public bool spring;
    public bool winter;

}
