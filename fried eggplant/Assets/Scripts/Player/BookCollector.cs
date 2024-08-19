using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollector : MonoBehaviour
{
    public GameObject bookPrefab;
    public Transform bookStackPosition;
    [SerializeField] public List<GameObject> collectedBooks = new List<GameObject>();

    public List<GameObject> getBooks() { return collectedBooks; }

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
        BookFollow.GetInstance().addPos(newBook.transform.localPosition.y);

    }
    public void RemoveBook(GameObject book) {
        if (collectedBooks.Contains(book)) {

            collectedBooks.Remove(book);

            // get position of book and see its index from the parent.
            // if its the very first underside of the book where its equal to the stackPos, then above of all book
            // it will fall down until it goes to the first stack pos
            DataManager.instance.removeBook();
            BookFollow.GetInstance().removePos(book.transform.position.y);

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