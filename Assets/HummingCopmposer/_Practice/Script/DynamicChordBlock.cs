using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChordBlock : MonoBehaviour {
    [SerializeField]
    public GameObject chordC;
    public GameObject chordDm;
    public GameObject chordEm;
    public GameObject chordF;
    public GameObject chordG;
    public GameObject chordAm;
    public GameObject chordBmf5;

    GameObject parent;

    public const string C = "c";
    public const string Dm = "Dm";
    public const string Em = "Em";
    public const string F = "F";
    public const string G = "G";
    public const string Am = "Am";
    public const string Bmf5 = "Bmf5";

    string[] chordScore = new string[] {C,C,C,C,G,G,G,G,
                                        Am,Am,Am,Am,Em,Em,Em,Em,
                                        F,F,F,F,C,C,C,C,
                                        F,F,F,F,G,G,G,G,
                                        C,C,C,C,G,G,G,G,
                                        Am,Am,Am,Am,Em,Em,Em,Em,
                                        F,F,G,G,C,C,Am,Am,
                                        F,F,G,G,C,C,C,G,
                                        Am,Am,Em,Em,F,F,C,C,
                                        Am,Am,Em,Em,F,F,C,C,
                                        Am,Am,Em,Em,F,F,C,C,
                                        Am,Am,Em,Em,F,G,C,C,
                                        C,C,G,G,Am,Am,F,F,
                                        C,C,G,G,Am,Am,F,F,
                                        C,C,G,G,Am,Am,F,F,
                                        C,C,G,G,Am,Am,F,F};

    int n = 0;
    // Use this for initialization
    void OnEnable () {
        parent = GameObject.Find("CubeP");
        GameObject tmp = chordC;
        foreach (Transform child in parent.gameObject.transform) {
            switch (chordScore[n]) {
                case C:
                    tmp = chordC;
                    break;
                case Dm:
                    tmp = chordDm;
                    break;
                case Em:
                    tmp = chordEm;
                    break;
                case F:
                    tmp = chordF;
                    break;
                case G:
                    tmp = chordG;
                    break;
                case Am:
                    tmp = chordAm;
                    break;
                case Bmf5:
                    tmp = chordBmf5;
                    break;
            }
            n++;


            var obj = GameObject.Instantiate(tmp, child.position, child.rotation);
            obj.name = "ChordC" + n;
            obj.gameObject.transform.localScale = new Vector3(
                obj.gameObject.transform.localScale.x * 0.8f,
                obj.gameObject.transform.localScale.y * 0.8f,
                obj.gameObject.transform.localScale.z * 0.8f);

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}

static class Chord {

}
