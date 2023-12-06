using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int width = 21; // 게임 보드의 너비
    public int height = 21; // 게임 보드의 높이
    private Tile[,] tiles; // 타일 배열

    void Start()
    {
        InitializeTiles();
    }

    // 타일 초기화
    void InitializeTiles()
    {
        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                tiles[x, z] = new Tile();
                tiles[x, z].owner = null; // 초기 소유자 없음
                tiles[x, z].position = new Vector3(x, 0, z); // 타일의 월드 위치
            }
        }
    }

    // 타일 소유권 변경
    public void ChangeTileOwner(int x, int z, GameObject owner)
    {
        if (x >= 0 && x < width && z >= 0 && z < height)
        {
            tiles[x, z].owner = owner;
        }
    }
}

// 타일 정보를 저장할 클래스
public class Tile
{
    public GameObject owner; // 타일 소유자
    public Vector3 position; // 타일의 월드 위치
}
