using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockTargetScript : MonoBehaviour {

    private List<GameObject> goList = new List<GameObject>();

    private float[] posX = new float[4];
    private float[] posZ = new float[4];

    //一小節を何等分するか
    [SerializeField, Range(1, 16)]
    int sepNum = 2;

    [SerializeField]
    GameObject targetCube;

    float cubeAngleY;
    float cubeAngleX;

    // Use this for initialization
    void Start () {

        GameObject go = GameObject.Find("CubeP");

        int num = 0;
        int c = 0;
        GameObject parent = GameObject.Find("Target");

        int i = 0;
        int tmp = 1; 

        //ターゲットオブジェクトをリストに入れる
        foreach (Transform child in parent.gameObject.transform) {
            num++;
            goList.Add(child.gameObject);
        }

        while ( c < num-1 ) {
            posX[0] = (goList[c].transform.position.x - goList[c + 1].transform.position.x);
            posZ[0] = (goList[c].transform.position.z - goList[c + 1].transform.position.z);
            i = 1;

            while ( i <= sepNum ) {
                GameObject cube2 = Instantiate(targetCube, this.transform.position, Quaternion.identity);
                //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.name = "cube" + c + i;
                //当たり判定用のrigidbodyを取り付け
                //cube2.gameObject.AddComponent<Rigidbody>();
                //cube2.GetComponent<Rigidbody>().useGravity = false;
                //cube2.GetComponent<BoxCollider>().isTrigger = true;

                cube2.transform.position = new Vector3((goList[c].transform.position.x - ((posX[0] / sepNum) / 2) * ( 1 + 2 * ( i - 1 ))),
                    1.42f,
                    (goList[c].transform.position.z - ((posZ[0] / sepNum) / 2) * (1 + 2 * (i - 1)))
                    );


                cube2.gameObject.transform.localScale = new Vector3(
                    cube2.gameObject.transform.localScale.x * 0.01f,
                    cube2.gameObject.transform.localScale.y * 0.01f,
                    cube2.gameObject.transform.localScale.z * 0.01f);

                cube2.transform.parent = go.transform;
                i++;
                
                cubeAngleY = 5.625f * tmp;
                //Debug.Log(hummingBlockAngleY);
                cubeAngleX = -90;
                cube2.transform.Rotate(new Vector3(cubeAngleX, cubeAngleY, 0));

            }
            c++;
            tmp += 2;

        }

        if (c == num - 1) {
            posX[0] = (goList[c].transform.position.x - goList[0].transform.position.x);
            posZ[0] = (goList[c].transform.position.z - goList[0].transform.position.z);
            i = 1;

            while (i <= sepNum) {
                GameObject cube2 = Instantiate(targetCube, this.transform.position, Quaternion.identity);
                //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.name = "cube" + c + i;
                cube2.transform.position = new Vector3((goList[c].transform.position.x - ((posX[0] / sepNum) / 2) * (1 + 2 * (i - 1))),
                    1.42f,
                    (goList[c].transform.position.z - ((posZ[0] / sepNum) / 2) * (1 + 2 * (i - 1)))
                    );


                cube2.gameObject.transform.localScale = new Vector3(
                    cube2.gameObject.transform.localScale.x * 0.01f,
                    cube2.gameObject.transform.localScale.y * 0.01f,
                    cube2.gameObject.transform.localScale.z * 0.01f);

                cube2.transform.parent = go.transform;
                i++;

                cubeAngleY = 5.625f * tmp;
                //Debug.Log(hummingBlockAngleY);
                cubeAngleX = -90;
                cube2.transform.Rotate(new Vector3(cubeAngleX, cubeAngleY, 0));

            }
            tmp += 2;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
