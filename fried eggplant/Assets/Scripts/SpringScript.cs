using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    [SerializeField] private float launchPower = 20f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            PlayerMovement player = collider.gameObject.GetComponent<PlayerMovement>();
            player.velocity += new Vector2(0, launchPower);
            StartCoroutine(springAnimation());
        }
    }
    IEnumerator springAnimation() {
        anim.SetBool("isTouched", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isTouched", false);
    }
}
