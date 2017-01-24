using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Leg : MonoBehaviour {

    Vector3 rotate_point;

    int Upper_Length = 1;
    //int Lower_Length = 1;
    // int Foot_Size = 1;

    Transform trans;

    Quaternion Leg_Start_Rot;
    //  Quaternion Knee_Start_Rot = Quaternion.identity;
    //  Quaternion Ankle_Start_Rot = Quaternion.identity;

    Vector3 Leg_Start_Pos;
    //  Vector3 Knee_Start_Pos = Quaternion.identity;
    //  Vector3 Angle_Start_Pos = Quaternion.identity;

    Move[] movements;
    int Cur_Move = 0;
    float time_since_last_change;
    Vector3 cur_rot_axis = Vector3.zero;
    float cur_rot_ang = 0.0f;

    void Start()
    {
        trans = gameObject.GetComponent(typeof(Transform)) as Transform;
        Leg_Start_Pos = trans.position;
        Leg_Start_Rot = trans.rotation;
        time_since_last_change = Time.time;
        rotate_point = Vector3.up * 0.4f;
    }

    public void Reset()
    {
        transform.rotation = Leg_Start_Rot;
        transform.position = Leg_Start_Pos;
    }

    public void SetMove(Move[] mv)
    {
        movements = mv;
    }

    public Move[] GetMove()
    {
        return movements;
    }

    void Update()
    {
        Quaternion.Slerp(movements[Cur_Move].ang, movements[1 - Cur_Move].ang, (Time.time - time_since_last_change) / movements[Cur_Move].time).ToAngleAxis(out cur_rot_ang, out cur_rot_axis);
        transform.RotateAround(transform.TransformPoint(rotate_point), cur_rot_axis * Time.deltaTime / movements[Cur_Move].time, cur_rot_ang * Time.deltaTime / movements[Cur_Move].time);
        //transform.rotation = Quaternion.Slerp(movements[Cur_Move].ang, movements[1-Cur_Move].ang, (Time.time - time_since_last_change)/movements[Cur_Move].time );
        if ((Time.time - time_since_last_change) / movements[Cur_Move].time > 1)
        {
            Cur_Move = 1-Cur_Move;
            time_since_last_change = Time.time;

        }
    }

    public static Leg Avg(Leg A,Leg B)
    {
        Leg output = new Leg();
        output.Upper_Length = (A.Upper_Length + B.Upper_Length) / 2;
        output.Leg_Start_Rot = Quaternion.Slerp(A.Leg_Start_Rot, B.Leg_Start_Rot, 0.5f);
        int MoveCount = (A.movements.Length + B.movements.Length) / 2;
        output.movements = new Move[MoveCount];
        for (int i = 0; i < MoveCount; i++)
        {
            if(i<A.movements.Length && i< B.movements.Length)
            {
                output.movements[i] = Move.Avg(A.movements[i], B.movements[i]);
            }
            else
            {
                if (i < A.movements.Length)
                {
                    output.movements[i] = A.movements[i];
                }
                if (i < B.movements.Length)
                {
                    output.movements[i] = B.movements[i];
                }
            }
        }

        return output;
    }

    public void Mutate()
    {
        //TODO: See If we will actually need this...
    }

}