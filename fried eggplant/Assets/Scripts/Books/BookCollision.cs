using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollision : MonoBehaviour
{
    private bool hasRigidbody = false;
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
        if (collision.transform.tag == "obstacle")
        {
            if (!hasRigidbody)
            {
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
                hasRigidbody = true;

                Vector2 forceDirection = collision.contacts[0].normal * -1;
                float forceMagnitude = 10f;

                rb.AddForce(forceDirection * forceMagnitude * 5, ForceMode2D.Impulse);

                StartCoroutine(destroyBook());
            }
        }
    }

    IEnumerator destroyBook()
    {
        transform.SetParent(null);
        float alphaVal = SpriteRenderer.color.a;
        Color tmp = SpriteRenderer.color;
        bookCollector.RemoveBook(gameObject);

        while (SpriteRenderer.color.a < 1)
        {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            SpriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
