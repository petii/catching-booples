using UnityEngine;
using System.Collections;

public class Blinking : MonoBehaviour {

    float blinkTime;
    float timer;

    // Use this for initialization
    void Start() {
        blinkTime = transform.parent.GetComponent<CreatureLogic>().blinkTime;
        timer = blinkTime;
    }

    // Update is called once per frame
    void Update() {
        if ( timer <= 0 ) {
            StartCoroutine( Trigger() );
            timer = blinkTime;
        }

        timer -= Time.deltaTime;
    }

    IEnumerator Trigger() {
        GetComponent<Animator>().SetBool( "blink", true );
        transform.GetChild( 0 ).GetComponent<Animator>().SetBool( "blink", true );
        yield return new WaitForSeconds( 0.1f );
        transform.GetChild( 0 ).GetComponent<Animator>().SetBool( "blink", false );
        GetComponent<Animator>().SetBool( "blink", false );
    }

    public void KilledOwner() {
        GetComponent<SpriteRenderer>().sortingOrder = -4;
        transform.GetChild( 0 ).GetComponent<SpriteRenderer>().sortingOrder = -3;
    }
}
