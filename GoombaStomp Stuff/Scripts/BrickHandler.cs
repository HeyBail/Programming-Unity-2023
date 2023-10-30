using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickHandler : MonoBehaviour
{
    protected Tilemap myTileMap;

    private bool respawnable;
    private GameManager gameManager;

    public int respawnCoolDown = 10;

    public virtual void HandleBonk(Vector2 worldPosition) 
    {
        Vector3Int gridPosition = myTileMap.WorldToCell(worldPosition);
        TileBase tile = myTileMap.GetTile(gridPosition);
        if (tile == null)
        {
            gridPosition = myTileMap.WorldToCell(worldPosition - new Vector2(0.5f, 0));
            tile = myTileMap.GetTile(gridPosition);
        }
        if (tile == null)
        {
            gridPosition = myTileMap.WorldToCell(worldPosition + new Vector2(0.5f, 0));
            tile = myTileMap.GetTile(gridPosition);
        }
        if (tile != null)
        {
            if (respawnable)
            {
                StartCoroutine(respawn(gridPosition, myTileMap.GetTile(gridPosition)));
            }
            myTileMap.SetTile(gridPosition, null);
        }
    }

    IEnumerator respawn(Vector3Int gridPosition, TileBase brick) 
    {
        yield return new WaitForSeconds(respawnCoolDown);
        myTileMap.SetTile(gridPosition, brick);
    }


    // Start is called before the first frame update
    void Start()
    {
        myTileMap = GetComponent<Tilemap>();
        gameManager = GameManager.FindFirstObjectByType<GameManager>();

        gameManager.setDifficulty((int)gameManager.difficulty);
    }

    public void setDifficulty(GameManager.Difficulty difficulty) 
    { 
        switch (difficulty)
        {
            case GameManager.Difficulty.ReallyEasy:
                respawnable = true;
                break;
            case GameManager.Difficulty.Easy:
                respawnable = false;
                break;
            case GameManager.Difficulty.Normal:
                respawnable = false;
                break;
        }
    }
}
