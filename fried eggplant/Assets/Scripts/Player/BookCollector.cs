using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollector : MonoBehaviour
{
    public static BookCollector GetInstance() { return instance; }
    public static BookCollector instance;
    public GameObject bookPrefab;
    public Transform bookStackPosition;
    [SerializeField] public List<GameObject> collectedBooks = new List<GameObject>();

    public List<GameObject> getBooks() { return collectedBooks; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Book"))
        {
            CollectBook(other.gameObject);
        }
    }

    void CollectBook(GameObject book)
    {
        book.SetActive(false);

        GameObject newBook = Instantiate(bookPrefab, bookStackPosition.position, Quaternion.identity, bookStackPosition);

        newBook.SetActive(true);

        newBook.transform.localPosition += new Vector3(Random.Range(-1, 2) * 1/16f, collectedBooks.Count * newBook.GetComponent<SpriteRenderer>().bounds.size.y, 0);

        collectedBooks.Add(newBook);
        DataManager.instance.addBook();
    }

    public void RemoveBook(GameObject book, bool makeDynamic)
    {
        if (collectedBooks.Contains(book))
        {
            // Remove this book and every book above it
            var index = collectedBooks.IndexOf(book);
            while (collectedBooks.Count > index) {
                // Keep removing the last book
                var b = collectedBooks[^1];
                if (makeDynamic) b.GetComponent<BookCollision>().MakeDynamic();
                collectedBooks.RemoveAt(collectedBooks.Count - 1);
                DataManager.instance.removeBook();
            }
        }
    }
    public int getNumBooks()
    {
        return collectedBooks.Count;
    }

    public IEnumerator RemoveTopBook()
    {
        GameObject book = collectedBooks[^1];
        RemoveBook(book, makeDynamic: false);
        Destroy(book);
        yield return null;
    }
}