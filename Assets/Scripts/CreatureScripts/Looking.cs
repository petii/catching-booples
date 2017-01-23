using UnityEngine;
using System.Collections;

public class Looking : MonoBehaviour {

    public float maxRadius;
    public float speed;

    float degree;
    float radius;

    public float timer = 0;
    bool moving = false;

    // Update is called once per frame
    void Update() {
        if ( timer <= 0 ) {
            moving = true;
            StartCoroutine( MoveToPos( FindPos() ) );
            timer = Random.Range( 0.5f, 3f );
        }
        if ( !moving )
            timer -= Time.deltaTime;
    }

    Vector3 FindPos() {
        degree = Mathf.Deg2Rad * Random.Range( 0, 360 );
        radius = Random.Range( 0.00f, maxRadius );
        float x = Mathf.Cos( degree );
        float y = Mathf.Sin( degree );

        Vector3 pos = new Vector3( x, y, 0 );
        pos.Normalize();
        pos.Scale( new Vector2( radius, radius ) );

        return pos;
    }

    IEnumerator MoveToPos( Vector3 pos ) {
        Vector3 tmp = transform.localPosition;

        float startTime = Time.time;


        float journeyLength = Vector3.Distance( tmp, pos );

        while ( (Time.time - startTime) * speed < journeyLength ) {
            float fracJourney = (Time.time - startTime) * speed / journeyLength;
            transform.localPosition = Vector3.Lerp( tmp, pos, fracJourney );
            yield return new WaitForEndOfFrame();
        }
        moving = false;
    }
}
