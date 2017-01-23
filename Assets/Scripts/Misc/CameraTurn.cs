using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTurn : MonoBehaviour {

    int turnSpeed;
    public List<int> turnSpeedRange;

    // Use this for initialization
    void Start() {
        NewSpeed();
    }

    // Update is called once per frame
    void Update() {
        if ( Mathf.Abs( GetComponent<Rigidbody2D>().angularVelocity ) < Mathf.Abs( turnSpeed ) ) {
            GetComponent<Rigidbody2D>().AddTorque( turnSpeed * Timer.deltaTime() );
        }
    }

    public void NewSpeed() {
        int spinDir = Random.Range( 0, 2 );
        if ( spinDir > 0 )
            spinDir = 1;
        else
            spinDir = -1;
        //GetComponent<Rigidbody2D>().angularVelocity = 0;
        turnSpeed = spinDir * Random.Range( turnSpeedRange[0], turnSpeedRange[1] );
    }
}
