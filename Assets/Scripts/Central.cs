using UnityEngine;

public class Central : MonoBehaviour {
    public enum GameMode {
        Survival,
        SuddenDeath,
        TimeAttack
    }

    public static GameMode MODE;

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad( transform.gameObject );
    }

    // Update is called once per frame
    void Update() {}

    //0 survival
    //1 sudden death
    //2 time attack
    public void StartGame( int mode ) {
        switch ( mode ) {
            case 0:
                MODE = GameMode.Survival;
                break;
            case 1:
                MODE = GameMode.SuddenDeath;
                break;
            case 2:
                MODE = GameMode.TimeAttack;
                break;

        }
        Application.LoadLevel( 1 );
    }

    public void GoToMenu() {
        Application.LoadLevel( "menu" );
    }

    public static void GoToSettings() {
        Application.LoadLevel( "settings" );
    }
}
