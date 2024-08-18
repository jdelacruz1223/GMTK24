using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollector : MonoBehaviour
{
    public GameObject bookPrefab;
    public Transform bookStackPosition;
    [SerializeField] private List<GameObject> collectedBooks = new List<GameObject>();

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

        GameObject newBook = Instantiate(bookPrefab, bookStackPosition.position, Quaternion.identity);

        newBook.transform.SetParent(bookStackPosition);

        newBook.SetActive(true);

        newBook.transform.localPosition += new Vector3(0, collectedBooks.Count * newBook.GetComponent<SpriteRenderer>().bounds.size.y, 0);

        collectedBooks.Add(newBook);
        DataManager.instance.addBook();
    }
    public void RemoveBook(GameObject book) {
        if (collectedBooks.Contains(book)) {
            collectedBooks.Remove(book);
            DataManager.instance.removeBook();
        }
    }
    public int getNumBooks(){
        return collectedBooks.Count;
    }

    public IEnumerator RemoveTopBook(){
        GameObject book = collectedBooks[collectedBooks.Count-1];
        RemoveBook(book);
        Destroy(book);
        yield return null;
    }
}