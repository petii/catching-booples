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
        var startbutt = transform.Find( "start" );
        if ( !startbutt.GetComponent<Toggle>().isOn ) {
            startbutt.GetComponent<Animator>().Play( "Shrink" );
            transform.Find( "SubButtonContainer" ).GetComponent<Animator>().Play( "Float Out" );
        }
        else {
            startbutt.GetComponent<Animator>().Play( "Expand" );
            transform.Find( "SubButtonContainer" ).GetComponent<Animator>().Play( "Float In" );
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
        var sett = transform.Find( "settings" ).gameObject;
        if (sett.activeSelf) {
            yield return new WaitForSeconds( 0.03f );
        }
        else {
            yield return new WaitForSeconds( 0.27f );
        }
        sett.SetActive( !sett.activeSelf );
    }

    IEnumerator SetContentActiveState() {
        var Content = transform.Find( "credits" ).Find( "CreditsText" ).gameObject;
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
            transform.Find( "credits" ).GetComponent<Animator>().Play( "Down" );
        }
        else {
            transform.Find( "credits" ).GetComponent<Animator>().Play( "Up" );
        }
        //StartCoroutine( "SetContentActiveState" );
        StartCoroutine( "CreditsOtherButtonsSetActive" );
        creditsUp = !creditsUp;
    }
}
