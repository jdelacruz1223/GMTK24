using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpawnManager : MonoBehaviour
{
    public GameObject spawnPoints;
    public GameObject smallBookPrefab;
    public GameObject mediumBookPrefab;
    public GameObject bigBookPrefab;

    private void Start()
    {
        SpawnBook();
    }

    /// <summary>
    /// TODO: Must add a check if what type of Tag the Book is
    /// </summary>
    void SpawnBook()
    {
        foreach (Transform child in spawnPoints.transform)
        {
            switch(child.tag)
            {
                case "SmlBookPoint":
                    SpawnFromPoint(child, smallBookPrefab);
                    break;
                case "MdBookPoint":
                    SpawnFromPoint(child, mediumBookPrefab);
                    break;
                case "BigBookPoint":
                    SpawnFromPoint(child, bigBookPrefab);
                    break;
            }
        }
    }

    void SpawnFromPoint(Transform child, GameObject prefab) => Instantiate(prefab, child.position, Quaternion.identity);
}
