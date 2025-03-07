using System.IO;

namespace SecureNotesPlus;

public class Note
{
    private string fileName;
    private bool isEncrypted = false;
    
    public Note(string fileName)
    {
        this.fileName = fileName;
    }

    public bool getIsEncrypted()
    {
        return isEncrypted;
    }

    public string getFileName()
    {
        return fileName;
    }

    public override string ToString()
    {
        return fileName;
    }
}