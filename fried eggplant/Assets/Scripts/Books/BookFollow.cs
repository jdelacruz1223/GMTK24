using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFollow : MonoBehaviour
{
    public static BookFollow GetInstance() { return me; }
    public static BookFollow me;

    [SerializeField] GameObject stackPos;
    public List<float> yPositions { get; private set; }
    private new Rigidbody2D rigidbody;

    public void Start()
    {
        yPositions = new List<float>();
        rigidbody = GetComponent<Rigidbody2D>();
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

    void FixedUpdate()
    {
      rigidbody.position = stackPos.transform.position;
    }
}
