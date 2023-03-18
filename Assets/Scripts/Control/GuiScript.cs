using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiScript : MonoBehaviour {
    RectTransform progress;
    RectTransform time;

    RectTransform eyes;
    RectTransform creatureCount;

    void Awake() {
        eyes = (RectTransform)transform.Find( "Canvas" ).Find( "Info" ).Find( "Background" ).Find( "eyeNum" );
        progress = (RectTransform)transform.Find( "Canvas" ).Find( "Info" ).Find( "Background" ).Find( "creatureNum" );
        time = (RectTransform)transform.Find( "Canvas" ).Find( "Info" ).Find( "Background" ).Find( "time" );
    }

    // Update is called once per frame
    void Update() {
        int sec = Mathf.FloorToInt( GetComponent<Logic>().time * 10 );
        int milisec = sec % 10;
        sec /= 10;
        time.GetComponent<Text>().text = (" " + sec.ToString() + "." + milisec.ToString());
    }

    public enum UpdateType { newObj, time, prog };

    public void UpdateGUI( UpdateType type ) {
        if ( type == UpdateType.time ) {
            time.GetComponent<Text>().text = GetComponent<Logic>().time.ToString();
        }
        else if ( type == UpdateType.newObj ) {
            eyes.GetComponent<Text>().text = Logic.Objective.eyeCount.ToString();
            progress.GetComponent<Text>().text = (Logic.Objective.killNum - Logic.Objective.Progress()).ToString();

        }
        else if ( type == UpdateType.prog ) {
            progress.GetComponent<Text>().text = (Logic.Objective.killNum - Logic.Objective.Progress()).ToString();
        }

    }
}
