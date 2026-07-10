using System;
using UnityEngine;

[Serializable]
public class TileEventSpawnConfig
{
    [SerializeField] private TileEventData eventData;
    [SerializeField] private int count;

    public TileEventData EventData => eventData;
    public int Count => count;
}