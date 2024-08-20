using UnityEngine;

public class BookFollow : MonoBehaviour
{
    [SerializeField] GameObject stackPos;

    void Update()
    {
        transform.position = stackPos.transform.position;
    }
}
