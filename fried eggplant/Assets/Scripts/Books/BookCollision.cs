using System.Collections;
using System.Collections.Generic;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("HeldBook")) {
            return;
        }

        if (!TryGetComponent<Rigidbody2D>(out _))
        {
            StartCoroutine(ThrowOffBook());
        }
    }

    public void MakeDynamic() {
        transform.SetParent(null);
        Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        Vector2 forceDirection = Random.insideUnitCircle;
        float forceMagnitude = 10f;
        rigidbody.AddForce(5 * forceMagnitude * forceDirection, ForceMode2D.Impulse);
            
    }

    IEnumerator ThrowOffBook()
    {
        bookCollector.RemoveBook(gameObject, makeDynamic: true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
