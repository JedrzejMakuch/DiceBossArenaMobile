using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private BoardTile tilePrefab;
    [SerializeField] private Transform boardParent;
    [SerializeField] private int tileCount = 16;

    private readonly List<BoardTile> generatedTiles = new();
    public List<BoardTile> GeneratedTiles => generatedTiles;

    private void Awake()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        generatedTiles.Clear();

        Vector3[] positions = GetLoopPositions();

        for (int i = 0; i < tileCount; i++)
        {
            BoardTile tile = Instantiate(
                tilePrefab,
                positions[i],
                Quaternion.identity,
                boardParent
            );

            tile.name = $"Tile_{i:00}";
            generatedTiles.Add(tile);
        }
    }

    private Vector3[] GetLoopPositions()
    {
        return new Vector3[]
   {
        new Vector3(-2.4f, -0.2f, 0), // 00
        new Vector3(-2.1f,  0.55f, 0), // 01
        new Vector3(-1.6f,  1.15f, 0), // 02
        new Vector3(-0.85f, 1.55f, 0), // 03

        new Vector3( 0.0f,  1.75f, 0), // 04

        new Vector3( 0.85f, 1.55f, 0), // 05
        new Vector3( 1.6f,  1.15f, 0), // 06
        new Vector3( 2.1f,  0.55f, 0), // 07
        new Vector3( 2.4f, -0.2f, 0), // 08

        new Vector3( 2.1f, -0.95f, 0), // 09
        new Vector3( 1.6f, -1.55f, 0), // 10
        new Vector3( 0.85f,-1.95f, 0), // 11

        new Vector3( 0.0f, -2.15f, 0), // 12

        new Vector3(-0.85f,-1.95f, 0), // 13
        new Vector3(-1.6f, -1.55f, 0), // 14
        new Vector3(-2.1f, -0.95f, 0), // 15
   };
    }
}