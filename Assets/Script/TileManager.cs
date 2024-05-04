using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager instance = null;
    private int row = 5;
    private int col = 10;
    private TileComponent[,] tiles; //비순차적 접근이 필요하므로 List 대신 배열 사용
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        tiles = new TileComponent[row, col];
    }

    private void Start()
    {
        TileComponent[] arrTiles = GetComponentsInChildren<TileComponent>();
        Array.Sort(arrTiles);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                tiles[r,c] = arrTiles[r * col + c];
                arrTiles[r * col + c].idx = r * col + c;
            }
        }
    }

    public static TileManager Instance
    {
        get
        {
            return instance;
        }
    }
}