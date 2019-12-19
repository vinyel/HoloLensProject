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

}
