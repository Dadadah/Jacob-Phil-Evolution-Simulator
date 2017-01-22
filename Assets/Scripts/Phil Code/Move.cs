using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public readonly Quaternion ang;
    public readonly float time;

    public Move(Quaternion _ang, float _t)
    {
        ang = _ang;
        time = _t;
    }

    public static Move Avg(Move A, Move B)
    {
        return new Move(Quaternion.Slerp(A.ang, B.ang, 0.5f), ((A.time + B.time) / 2) + (Random.value-0.5f));
    }

}
