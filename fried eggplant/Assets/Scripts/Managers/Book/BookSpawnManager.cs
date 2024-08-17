using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpawnManager : MonoBehaviour
{
    public GameObject spawnPoints;
    public GameObject bookPrefab;

    private void Start()
    {
        SpawnBook();
    }

    void SpawnBook()
    {
        foreach (Transform child in spawnPoints.transform)
        {
            Instantiate(bookPrefab, child.position, Quaternion.Euler(0, 0, 90));
        }
    }
}
