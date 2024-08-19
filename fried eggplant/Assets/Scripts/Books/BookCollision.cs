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
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) {
            transform.SetParent(null);
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            
            Vector2 forceDirection = collision.contacts[0].normal * -1;
            float forceMagnitude = 10f;
            rigidbody.AddForce(forceDirection * forceMagnitude * 5, ForceMode2D.Impulse);
            StartCoroutine(destroyBook(collision.gameObject));
        }
    }

    IEnumerator destroyBook(GameObject obj)
    {
        transform.SetParent(null);
       
        bookCollector.RemoveBook(gameObject);
        BookFollow.GetInstance().removePos(obj.transform.localPosition.y);

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
