using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Phil_Leg : MonoBehaviour {
    Leg me = new Leg();

}

class Leg
{
    int Upper_Length = 1;
    //int Lower_Length = 1;
   // int Foot_Size = 1;

    Quaternion Leg_Start_Rot = Quaternion.identity;
  //  Quaternion Knee_Start_Rot = Quaternion.identity;
  //  Quaternion Ankle_Start_Rot = Quaternion.identity;

    Quaternion Leg_Curr_Rot;
    //Quaternion Knee_Curr_Rot;
    //Quaternion Ankle_Curr_Rot;

    Move[] movements;
    int Cur_Move = 0;

    public Leg()
    {
        Leg_Curr_Rot =Leg_Start_Rot;
       // Knee_Curr_Rot = Knee_Start_Rot;
       // Ankle_Curr_Rot = Ankle_Start_Rot;
    }

}


class Move
{
    Quaternion  ang;
    float       time;

    public Move(Quaternion _ang, float _t){
        ang = _ang;
        time = _t;
    }

    public static Move Avg(Move A,Move B)
    {
        return new Move(Quaternion.Slerp(A.ang, B.ang, 0.5f), (A.time + B.time) / 2);
    }

}
