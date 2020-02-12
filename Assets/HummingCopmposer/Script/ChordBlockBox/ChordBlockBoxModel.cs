using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class ChordBlockBoxModel : MonoBehaviour
{
    private float angleMax = 90;
    public float AngleMax
    {
        get { return angleMax; }
    }
    private FloatReactiveProperty boxAngle = new FloatReactiveProperty();
    public FloatReactiveProperty Angle
    {
        get { return boxAngle; }
    }

    // ボックスからブロックを落とす処理の変数
    private IntReactiveProperty shakeCount = new IntReactiveProperty();
    public IntReactiveProperty ShakeCount => shakeCount;
    
    private Vector3 acceleration;
    public Vector3 Acceleration
    {
        get { return acceleration; }
        set { acceleration = value; }
    }

    private Vector3 preAcceleration;
    public Vector3 PreAcceleration
    {
        get { return preAcceleration; }
        set { preAcceleration = value; }
    }

    private float dotProduct;
    public float DotProduct
    {
        get { return dotProduct; }
        set { dotProduct = value; }
    }
}
