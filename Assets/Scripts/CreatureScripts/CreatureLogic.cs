using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CreatureLogic : MonoBehaviour {

    public float blinkTime;

    public int eyeCount;

    //public static bool Exists = false;

    float speed = 1;
    public List<int> speedRange;

    float spinSpeed = 1;
    public List<int> spinSpeedRange;

    public float radius = 10;

    float scaleAmount;
    public List<float> scaleRange;

    public float mapBounds;

    // Use this for initialization
    void Start() {
        eyeCount = this.transform.childCount;

        blinkTime = blinkTime * Random.Range( 0.5f, 1.5f );

        int spinDir = Random.Range( 0, 2 );
        if ( spinDir > 0 )
            spinDir = 1;
        else
            spinDir = -1;

        EventTrigger ter = GetComponent<EventTrigger>();
        UnityAction<BaseEventData> ua = new UnityAction<BaseEventData>( NewClickHandling );
        ter.delegates[0].callback.AddListener( ua );

        spinSpeed = spinDir * Random.Range( spinSpeedRange[0], spinSpeedRange[1] );

        scaleAmount = Random.Range( scaleRange[0], scaleRange[1] );
        Vector3 scale = new Vector3( scaleAmount,
                                    scaleAmount,
                                    0 );
        transform.localScale = scale;

        float deg = Random.Range( 0, 360 ) * Mathf.Deg2Rad;

        Vector3 pos = new Vector3( Mathf.Cos( deg ),
                                   Mathf.Sin( deg ) );
        pos.Scale( new Vector3( radius, radius ) );
        transform.localPosition = pos;

        speed = Random.Range( speedRange[0], speedRange[1] );
        GetComponent<Rigidbody2D>().angularVelocity = spinSpeed / 2;

    }

    bool vanishing = false;

    // Update is called once per frame
    void Update() {
        if ( !vanishing ) {
            if ( Mathf.Abs( GetComponent<Rigidbody2D>().angularVelocity ) < Mathf.Abs( spinSpeed ) ) {
                GetComponent<Rigidbody2D>().AddTorque( spinSpeed * Timer.deltaTime() );
            }
            if ( GetComponent<Rigidbody2D>().velocity.magnitude < speed ) {
                Vector2 force = ForceDirection();
                force.Scale( new Vector2( speed, speed ) );
                GetComponent<Rigidbody2D>().AddForce( force );
            }
            if ( Mathf.Abs( transform.position.x ) > mapBounds || Mathf.Abs( transform.position.y ) > mapBounds ) {
                Death( TypeOfDeath.outOfBounds );
            }
        }
    }

    Vector2 ForceDirection() {
        Vector3 locPos = transform.localPosition;
        Vector2 force = new Vector2( -locPos.x, -locPos.y );
        force.Normalize();
        return force;
    }

    public enum TypeOfDeath { hit, outOfBounds, outOfTime, objComplete };

    public void NewClickHandling( UnityEngine.EventSystems.BaseEventData baseEvent ) {
        Death( TypeOfDeath.hit );
    }

    public void Death( TypeOfDeath tod = TypeOfDeath.objComplete ) {
        vanishing = true;
        transform.position = new Vector3( transform.position.x,
                                          transform.position.y,
                                          1 );
        GetComponent<CircleCollider2D>().enabled = false;
        Camera.main.GetComponent<CameraTurn>().NewSpeed();
        if ( tod == TypeOfDeath.hit ) {
            //print("Gotcha!");
            transform.parent.parent.GetComponent<Logic>().GotOne( eyeCount );
        }

        if ( tod != TypeOfDeath.objComplete )
            this.transform.parent.parent.GetComponent<Logic>().AddCreature();

        GetComponent<Rigidbody2D>().AddTorque( spinSpeed );

        Vector2 dir = ForceDirection();
        dir.Scale( new Vector2( Mathf.Pow( speed, 2 ), Mathf.Pow( speed, 2 ) ) );
        int sig = Random.Range( 0, 1 ) * 2 - 1;
        float tmp = dir.x;
        dir.x = -sig * dir.y;
        dir.y = sig * tmp;
        GetComponent<Rigidbody2D>().AddForce( dir );

        StartCoroutine( Shrink() );
    }

    IEnumerator Shrink() {
        GetComponent<SpriteRenderer>().sortingOrder = -5;
        foreach ( Blinking e in GetComponentsInChildren<Blinking>() ) {
            e.KilledOwner();
        }
        while ( transform.localScale.x > 0 ) {
            transform.localScale = new Vector3( transform.localScale.x - 0.01f,
                                               transform.localScale.x - 0.01f,
                                               0 );
            yield return new WaitForEndOfFrame();
        }
        Destroy( this.gameObject );
    }
}
