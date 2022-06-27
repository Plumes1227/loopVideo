using UnityEngine;
using System.IO;
using System;

public class RuntimeText : MonoBehaviour
{
    public static void WriteString(string txtDataName, string message)
    {
        string path = Application.streamingAssetsPath + $"/{txtDataName}.txt";        
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(message);
        writer.Close();
    }
    public static string ReadString(string txtDataName)
    {
        string path = Application.streamingAssetsPath + $"/{txtDataName}.txt";
        try
        {
            StreamReader reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }
}