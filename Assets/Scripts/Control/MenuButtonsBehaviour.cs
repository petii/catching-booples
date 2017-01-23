using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartPress() {
        var startbutt = transform.FindChild( "start" );
        if ( !startbutt.GetComponent<Toggle>().isOn ) {
            startbutt.GetComponent<Animator>().Play( "Shrink" );
            transform.FindChild( "SubButtonContainer" ).GetComponent<Animator>().Play( "Float Out" );
        }
        else {
            startbutt.GetComponent<Animator>().Play( "Expand" );
            transform.FindChild( "SubButtonContainer" ).GetComponent<Animator>().Play( "Float In" );
        }
    }

    IEnumerator WaitForIt() {
        yield return new WaitForSeconds(0.4f);
        Central.GoToSettings();
    }

    public void SettingsPress() {
        /*transform.FindChild( "settings" ).GetComponent<Animator>().Play( "Up" );
        StartCoroutine( "WaitForIt" );*/
    }

    IEnumerator CreditsOtherButtonsSetActive() {
        var sett = transform.FindChild( "settings" ).gameObject;
        if (sett.activeSelf) {
            yield return new WaitForSeconds( 0.03f );
        }
        else {
            yield return new WaitForSeconds( 0.27f );
        }
        sett.SetActive( !sett.activeSelf );
    }

    IEnumerator SetContentActiveState() {
        var Content = transform.FindChild( "credits" ).FindChild( "CreditsText" ).gameObject;
        if ( creditsUp ) {
            yield return new WaitForSeconds( 0.35f );
        }
        else {
            yield return new WaitForEndOfFrame();
        }
        Content.SetActive( !Content.activeSelf );
    }

    public static bool creditsUp = false;
    
    public void CreditsPress() {
        if (creditsUp) {
            transform.FindChild( "credits" ).GetComponent<Animator>().Play( "Down" );
        }
        else {
            transform.FindChild( "credits" ).GetComponent<Animator>().Play( "Up" );
        }
        //StartCoroutine( "SetContentActiveState" );
        StartCoroutine( "CreditsOtherButtonsSetActive" );
        creditsUp = !creditsUp;
    }
}
