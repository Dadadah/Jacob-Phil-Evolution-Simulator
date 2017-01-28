using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public Quaternion ang;
    public float time;

    public Move()
    {
        ang = Random.rotation;
        time = Mathf.Clamp(Random.value, 0.01f, 1) * 10;
    }

    public Move(Quaternion _ang, float _t)
    {
        ang = _ang;
        time = _t;
    }

    public void RandomizeTime()
    {
        time = Random.value;
    }

    public void RandomizeAngle()
    {
        ang = Quaternion.Slerp(ang, Random.rotation, Random.value);
    }

    public static Move Avg(Move A, Move B)
    {
        return new Move(Quaternion.Slerp(A.ang, B.ang, 0.5f), (A.time + B.time) / 2);
    }

    public static Move RandAvg(Move A, Move B)
    {
        return new Move(Quaternion.Slerp(A.ang, B.ang, Random.value), ((A.time + B.time) / 2) + (Random.value - 0.5f));
    }

}
