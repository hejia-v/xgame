using System.IO;

public class ResourceManager
{

    public static string readTextFromFile(string filepath)
    {
        if (!File.Exists(filepath))
        {
            return null;
        }

        StreamReader sr = new StreamReader(filepath);

        if (sr == null)
        {
            return null;
        }
        string text = sr.ReadToEnd();

        return text;
    }
}
