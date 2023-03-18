using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupLogic : MonoBehaviour {

    public GameObject eyeObject;

    Transform eyeContainer;
    Transform text;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ShowYourself( int eyeNumber ) {


        text = transform.Find( "Background" ).Find( "Number" );


        GetComponent<Animator>().Play( "Appear" );

        // This part is probably not needed
        eyeContainer = transform.Find( "Background" ).Find( "Eyes" );
        eyeContainer.GetComponent<Rigidbody2D>().isKinematic = true;
        eyeContainer.GetComponent<Rigidbody2D>().isKinematic = false;
        int va = ((Random.Range( 0, 2 ) * 2) - 1) * Random.Range( 40, 80 );
        eyeContainer.GetComponent<Rigidbody2D>().AddTorque( va );

        var children = new List<GameObject>();
        foreach ( Transform child in eyeContainer )
            children.Add( child.gameObject );
        children.ForEach( child => Destroy( child ) );

        float rotation = 360 / eyeNumber;
        text.GetComponent<Text>().text = eyeNumber.ToString();
        for ( int i = 0 ; i < eyeNumber ; ++i ) {
            GameObject prevEye = (GameObject)Instantiate( eyeObject );
            RectTransform rekt = prevEye.GetComponent<RectTransform>();
            rekt.SetParent( eyeContainer );
            rekt.localPosition = new Vector3( 0, 0, 0 );
            rekt.localRotation = Quaternion.Euler( 0, 0, i*rotation );
            rekt.localScale = new Vector3( 1, 1, 1 );
        }

        
    }
}
