using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHelper : MonoBehaviour
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Tilemap _cropsMap;

    private void Start()
    {
        GameManager.Instance.GetTileManager().SetMap(_tileMap);
        GameManager.Instance.GetCropsManager().SetMap(_cropsMap);
        StartCoroutine(lateStart());
    }
    
    public IEnumerator lateStart()
    {
        while (GameManager.Instance == null || GameManager.Instance.GetPlantManager() == null ||
               GameManager.Instance.GetPlantManager()._plants == null)
        {
            yield return new WaitForSeconds(0.25f);
        }

        GameManager.Instance.GetCropsManager().RestorePlantMap();
        GameManager.Instance.GetCropsManager().RestoreTileMap();
    }

    private void OnDisable()
    {
        // Debug.Log("Saving Tiles for scene " + (int) SceneLoader.Instance.previousScene);
        // SaveManager.Instance.UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) SceneLoader.Instance.previousScene);
    }
}
