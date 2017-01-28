using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite
{
    public static void StartFile()
    {

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[5];
        List<string[]> rowData = new List<string[]>();
        rowDataTemp[0] = "Generation";
        rowDataTemp[1] = "Min";
        rowDataTemp[2] = "Average";
        rowDataTemp[3] = "Max";
        rowDataTemp[4] = "Standard Deviation";
        rowData.Add(rowDataTemp);


        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.Append(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    public static void Save(string[] info)
    {

        List<string[]> rowData = new List<string[]>();

        rowData.Add(info);


        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.Append(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = new StreamWriter(filePath, true);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    static string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }
}
