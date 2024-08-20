using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameObject player;
    public int extraJumpAmount = 0;
    public bool canBounce;
    public Vector2 instantBoostAmount = new Vector2(0, 0);
    public int booksAmount = 0;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }

    void Update()
    {

    }
    public Vector2 applyBoost()
    {
        Vector2 temp = instantBoostAmount;
        instantBoostAmount = new Vector2(0, 0);
        return temp;
    }
    public bool attemptToJumpAgain()
    {
        //returns if jump can be applied
        bool attempt = extraJumpAmount > 0;
        extraJumpAmount -= (extraJumpAmount > 0) ? 1 : 0;
        return attempt;
    }
    public void addJump()
    {
        extraJumpAmount += 1;
    }
    public void makeBouncey()
    {
        canBounce = true;
    }
    public void addBoost(Vector2 amt)
    {
        instantBoostAmount += amt;
    }
    public void addBook()
    {
        booksAmount += 1;
    }
    public void removeBook()
    {
        booksAmount -= (booksAmount > 0)?1:0;
    }
    public void removeBooks(int amt){
        BookCollector temp = player.GetComponent<BookCollector>();
        for (int i = 0; i < amt; i++){
            if(temp.getNumBooks() > 0){
                StartCoroutine(temp.RemoveTopBook());
            }
            else{
                removeBook();
            }
        }
    }
}