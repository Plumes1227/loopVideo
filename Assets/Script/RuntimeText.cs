using UnityEngine;
using System.IO;
using System;

public class RuntimeText : MonoBehaviour
{
    public static void WriteString(string txtDataName, string message)
    {
        string path = Application.streamingAssetsPath + $"/{txtDataName}.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(message);
        writer.Close();
        // StreamReader reader = new StreamReader(path);
        // //Print the text from the file
        // Debug.Log(reader.ReadToEnd());
        // reader.Close();
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
            return "";
        }

        //string path = Application.persistentDataPath + "/test.txt";
        //Read the text from directly from the test.txt file
        // reader.Close();
    }
}