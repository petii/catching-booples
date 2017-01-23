using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour {

    public static bool on = false;

	// Use this for initialization
	void Start () {
        on = transform.parent.FindChild( "Info" ).GetComponent<Toggle>().isOn;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RollDown() {
        on = !on;
        if ( !on ) {
            print( "now up" );
			GetComponent<Animator>().Play( "Up" );
			transform.parent.parent.GetComponent<Logic>().Continue();
        }
        else {
            print( "now down" );
            GetComponent<Animator>().Play( "Down" );
            transform.parent.parent.GetComponent<Logic>().Pause();
        }
    }

    public void LosingDrop() {
        GetComponent<Animator>().Play( "LostDrop" );
    }
}
