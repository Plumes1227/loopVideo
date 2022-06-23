using UnityEngine;
using System.IO;
public class RuntimeText : MonoBehaviour
{
    public static void WriteString(string txtDataName , string message)
    {
        string path = Application.persistentDataPath + $"/{txtDataName}.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(message);
        writer.Close();
        // StreamReader reader = new StreamReader(path);
        // //Print the text from the file
        // Debug.Log(reader.ReadToEnd());
        // reader.Close();
    }
    public static string ReadString(string txtDataName)
    {
        string path = Application.persistentDataPath + $"/{txtDataName}.txt";
        
        //string path = Application.persistentDataPath + "/test.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();
        // reader.Close();
    }
}