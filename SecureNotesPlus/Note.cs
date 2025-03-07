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

    public bool getIsEncrypted(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        FileInfo fileInfo = new FileInfo(filePath);
    
        // AES block size is 16 bytes
        return fileInfo.Length % 16 == 0;    }

    public string getFileName()
    {
        return fileName;
    }

    public override string ToString()
    {
        return fileName;
    }
}