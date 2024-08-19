using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class BookCollision : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D boxCollider;
    private BookCollector bookCollector;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        bookCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<BookCollector>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int index = transform.GetSiblingIndex();
        Vector3 lastPos = transform.position;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            transform.SetParent(null);
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;

            Vector2 forceDirection = collision.contacts[0].normal * -1;
            float forceMagnitude = 10f;
            rigidbody.AddForce(forceDirection * forceMagnitude * 5, ForceMode2D.Impulse);
            StartCoroutine(destroyBook(index, lastPos));
        }
    }

    IEnumerator destroyBook(int index, Vector3 lastPos)
    {
        bookCollector.RemoveBook(gameObject);
        SkillsUIManager.GetInstance().UpdateVisualAbility();
        BookFollow.GetInstance().UpdateStack(index, lastPos);

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
