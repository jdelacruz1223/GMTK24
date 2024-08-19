using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFollow : MonoBehaviour
{
    public static BookFollow GetInstance() { return me; }
    public static BookFollow me;

    [SerializeField] GameObject stackPos;
    public List<float> yPositions { get; private set; }

    public void Start()
    {
        yPositions = new List<float>();
    }

    private void Awake()
    {
        if (me != null)
        {
            Destroy(this);
            return;
        }

        me = this;
    }

    public void addPos(float pos) => yPositions.Add(pos);
    public void removePos(float pos) => yPositions.Remove(pos);

    void Update()
    {
        StartCoroutine(UpdateBookStack());
    }

    IEnumerator UpdateBookStack()
    {
        int index = 0;
        foreach(Transform child in transform)
        {
            Vector3 worldPosition = stackPos.transform.position;
            child.position = new Vector3(
                worldPosition.x,
                worldPosition.y + yPositions[index],
                child.position.z
            );
            index++;
        }

        yield return new WaitForEndOfFrame();
    }
}
