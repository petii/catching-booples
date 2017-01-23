using UnityEngine;
using System.Collections.Generic;

public class CreatureAppearance : MonoBehaviour {

    public List<Sprite> choices;

    // Use this for initialization
    void Start() {
        int whichOne = Random.Range( 0, choices.Count );
        GetComponent<SpriteRenderer>().sprite = choices[whichOne];
    }

    // Update is called once per frame
    void Update() {

    }
}
