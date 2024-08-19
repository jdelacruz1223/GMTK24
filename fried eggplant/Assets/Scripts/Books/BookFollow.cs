using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // update stack positions
    void Update()
    {

    }


    public void UpdateStack(int index, Vector3 lastPos) => StartCoroutine(UpdateBookStack(index, lastPos));

    IEnumerator UpdateBookStack(int index, Vector3 lastPos)
    {
        if (index == 0)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Rigidbody2D>() == null)
                {
                    Rigidbody2D rb = child.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Dynamic;

                    BookCollector.GetInstance().RemoveBook(child.gameObject);
                }
            }
        }

        SkillsUIManager.GetInstance().UpdateVisualAbility();
        yield return new WaitForEndOfFrame();
    }
}
