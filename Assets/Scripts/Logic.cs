using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Logic : MonoBehaviour {
    public static bool lost = false;
    public int misses = 0;

    public List<GameObject> creatures;
    public int creatureSum;
    Transform creatureContainer;
    public Dictionary<int, List<GameObject>> creaturesByEyes;

    //0-1: szemek
    //2-3: db
    public List<int> pool;

    public bool hasObjective;

    public static float points  = 0;
    public static int completedObjectives = 0;

    public float time;

    public static class Objective {
        public static int killNum;
        public static int eyeCount;
        static int progress;

        public static void NewMission( int killNumber, int eyesNumber ) {
            killNum = killNumber;
            eyeCount = eyesNumber;
            progress = 0;

        }

        public static void MadeProgress() {
            ++progress;
        }
        public static int Progress() { return progress; }
        public static bool Complete() { return progress >= killNum; }
    }

    public void GotOne( int numOfEyes ) {
        if ( numOfEyes == Objective.eyeCount ) {
            Objective.MadeProgress();
            GetComponent<GuiScript>().UpdateGUI( GuiScript.UpdateType.prog );
        }
        else {
            if ( Central.MODE == Central.GameMode.SuddenDeath ) {
                Lost();
            }
            else {
                ++misses;
                time -= misses;
            }
        }
        if ( Objective.Complete() ) {
            ++completedObjectives;

            if ( Central.MODE != Central.GameMode.TimeAttack ) {
                //todo: figure out how many time to add
                time += Objective.killNum;
            }

            for ( int i = 0 ; i < creatureContainer.childCount ; ++i ) {
                creatureContainer.GetChild( i ).GetComponent<CreatureLogic>().Death( CreatureLogic.TypeOfDeath.objComplete );
            }

            hasObjective = false;
        }
    }


    void Awake() {
        creaturesByEyes = new Dictionary<int, List<GameObject>>();
        foreach ( var v in creatures ) {
            int eyes = v.GetComponent<CreatureLogic>().eyeCount;
            List<GameObject> val = new List<GameObject>();

            if ( creaturesByEyes.TryGetValue( eyes, out val ) ) {
                val.Add( v );
            }
            else {
                creaturesByEyes.Add( eyes, new List<GameObject> { v } );
            }
        }

        /*foreach (var v in creaturesByEyes)
        {
            print(v.Key.ToString());
            foreach (var cre in v.Value)
            {
                print(cre.ToString());
            }
        }*/
    }

    public Transform canvas;

    // Use this for initialization
    void Start() {
        canvas = transform.FindChild( "Canvas" );

        lost = false;

        creatureContainer = transform.FindChild( "Creatures" ).transform;

        if ( Central.MODE == Central.GameMode.TimeAttack ) {
            time = 60f;
        }

        //print( creaturesByEyes.Count.ToString() );

        //points = 50000;
        NewObjective();
    }

    void NewObjPopup() {
        canvas.FindChild( "Popup" ).GetComponent<PopupLogic>().ShowYourself( Objective.eyeCount );
    }

    bool RandomBool() {
        return (Random.Range( 0, 2 ) == 0) ? false : true;
    }

	IEnumerator Summoner() {
		int objCreatureCount = 0;
		int creatureCount = 0;
		bool whatToAdd = RandomBool();
		/* not too great
		while (creatureCount < creatureSum - Objective.killNum) {
			AddCreature(false);
			++creatureCount;
			yield return new WaitForEndOfFrame();
		}
		while (objCreatureCount < Objective.killNum) {
			AddCreature(true);
			++objCreatureCount;
			yield return new WaitForEndOfFrame();
		}*/
		//maybe too easy?
		while (objCreatureCount <= Objective.killNum) {
            if (Timer.running) {
                AddCreature( whatToAdd );
                ++creatureCount;
                if (whatToAdd) {
                    ++objCreatureCount;
                }
                whatToAdd = RandomBool();
            }
			yield return new WaitForEndOfFrame();
		}
		while ( creatureCount < creatureSum ) {
            if (Timer.running) {
                AddCreature( false );
                ++creatureCount;
            }
			yield return new WaitForEndOfFrame();
		}
	}

    int RandomEyeNumber() {
        List<int> nums = new List<int>(creaturesByEyes.Keys);
        return nums[Random.Range(0, nums.Count)];
    }

    void NewObjective() {
        hasObjective = true;
        Objective.NewMission(   
                Random.Range( pool[0], pool[1] ), 
                RandomEyeNumber()
            );
		StartCoroutine (Summoner());

        NewObjPopup();
        GetComponent<GuiScript>().UpdateGUI( GuiScript.UpdateType.newObj );
    }


    // Update is called once per frame
    void Update() {
        if ( !hasObjective ) {
            NewObjective();
        }

        //points -= minusPointsPerSec * Time.deltaTime;
        if ( !lost ) {
            time -= 1 * Timer.deltaTime();
            if ( time < 0 ) { Lost(); }
        }
    }

    public void Reload() {
        Application.LoadLevel( Application.loadedLevel );
    }

    public void AddCreature( bool target ) {
        if ( !target ) {
            GameObject ch = (GameObject)Instantiate( creatures[Random.Range( 0, creatures.Count )] );
            ch.transform.parent = creatureContainer;
        }
        else {
            var tmpList = creaturesByEyes[Objective.eyeCount];
            GameObject cr = (GameObject)Instantiate( tmpList[Random.Range( 0, tmpList.Count )] );
            cr.transform.parent = creatureContainer;
        }
    }
    
    public void Lost() {
        var dropdown = transform.FindChild( "Canvas" ).FindChild( "Dropdown" );
        dropdown.FindChild( "Restart" ).gameObject.SetActive( true );
        dropdown.GetComponent<DropdownScript>().LosingDrop();
        var pts = dropdown.FindChild( "points" );
        var objComp = dropdown.FindChild( "compTask" );
        pts.FindChild( "Text" ).GetComponent<Text>().text = points.ToString();
        objComp.FindChild( "Text" ).GetComponent<Text>().text = completedObjectives.ToString();



        transform.FindChild( "Canvas" ).FindChild( "Info" ).GetComponent<Toggle>().enabled = false;
        lost = true;
        time = 0;

        points = 0;
        completedObjectives = 0;
        //something
    }

	void CreaturesMovement(bool disable) {
		var Container = transform.FindChild ("Creatures");
		for (int i=0; i<Container.childCount; ++i) {
			Container.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = disable;
			Container.GetChild(i).GetComponent<CircleCollider2D>().enabled = !disable;
		}
	}

    public void Pause() {
        print( "Should be paused!" );
		Timer.running = false;
		Camera.main.GetComponent<Rigidbody2D> ().isKinematic = true;
		CreaturesMovement (true);
    }

	public void Continue() {
		Timer.running = true;
		Camera.main.GetComponent<Rigidbody2D> ().isKinematic = false;
		CreaturesMovement (false);
	}
}
