using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//http://d.hatena.ne.jp/shinriyo/20130730/p3

public class MeshScriptv01 : MonoBehaviour {
    
    List<GameObject> tmpList = new List<GameObject>();
    List<GameObject> listGO = new List<GameObject>();

    //バーのターゲットを動的配置する

    // Use this for initialization
    void Awake() {

        /////////ターゲットを配置する
        Matrix4x4 thisMatrix = transform.localToWorldMatrix;

        Vector3 CenterOfBandVec = GameObject.Find("CenterOfBand").transform.position;

        float deg = 0;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Debug.Log(mesh);
        int i = 0;
        foreach (Vector3 vertex in vertices) {
            
            Vector3 vec = thisMatrix.MultiplyPoint3x4(vertex);
            if (vec.y - CenterOfBandVec.y <= 0.1f && vec.y - CenterOfBandVec.y >= -0.1f) {
                
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject cube = new GameObject();
                if (vec.x - CenterOfBandVec.x >= 0 && vec.z - CenterOfBandVec.z >= 0) {
                    deg = 0;
                }

                else if (vec.x - CenterOfBandVec.x >= 0 && vec.z - CenterOfBandVec.z < 0) {
                    deg = 180;
                }

                else if (vec.x - CenterOfBandVec.x < 0 && vec.z - CenterOfBandVec.z < 0) {
                    deg = 180;
                }

                //stickのある場所が必ずtg01になるためのおまじない
                else if (vec.x - CenterOfBandVec.x < 0 && vec.x - CenterOfBandVec.x >= -0.1f && vec.z - CenterOfBandVec.z >= 0) {
                    deg = 0;
                }

                else if (vec.x - CenterOfBandVec.x < 0 && vec.z - CenterOfBandVec.z >= 0) {
                    deg = 360;
                }

                float ftmp = 180 * Mathf.Atan(vec.x / vec.z) / Mathf.PI + deg;
                int itmp = (int)ftmp;
                


                //Debug.Log("mesh1 vertex at " + thisMatrix.MultiplyPoint3x4(vertex));
                Transform cubeTrans = cube.transform;
                cubeTrans.localPosition = vec;
                cubeTrans.localScale = Vector3.one * 0.03f;
                //２桁の場合、先頭に0角度で名前付け
                if ( itmp < 100 ) {
                    cube.name = "0" + itmp.ToString();
                }
                else {
                    cube.name = itmp.ToString();
                }
                
                tmpList.Add(cube);

            }
        }

        //名前でソートグルーっと円環状に名前をつけるため
        tmpList.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));
        int j = 1;
        //なぜか２つ同じ場所にオブジェクトが生成されるため、重複を消す
        //ついでに空オブジェクトのTargetにターゲットオブジェクトを子として入れる
        GameObject tgParent = GameObject.Find("Target");
        foreach (GameObject tmp in tmpList) {
            if ( i % 2 == 0 ) {
                tmp.transform.parent = tgParent.transform;
                if (j<10) {
                    tmp.name = "tg0" + j;
                }
                else {
                    tmp.name = "tg" + j;
                }
                
                j++;
                listGO.Add(tmp);
            }
            else {
                Destroy(tmp);

            }
            i++;
        }
       
    }
	
    

	// Update is called once per frame
	void Update () {
		
	}
}
