using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SecureNotesPlus;

public partial class EditWindow : Window
{
    string exeDir = AppDomain.CurrentDomain.BaseDirectory;
    private string _filePath;
    private readonly EncryptionService _encryptionService = new();
    
    public EditWindow(Note noteInput)
    {
        InitializeComponent();
        _filePath = exeDir+@"\Notes\" + noteInput.getFileName();
        LoadFileContent();
    }
    
    private void LoadFileContent()
    {
        if (File.Exists(_filePath))
        {
            using (FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                EditNoteBox.Text = streamReader.ReadToEnd();
            }
        }
    }

    public void writeAllText(string text)
    {
        using (FileStream fileStream = new FileStream(_filePath, FileMode.Open,
                   FileAccess.Write, FileShare.ReadWrite))
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(text);
        }
    }

    private void SaveOnClick(object? sender, RoutedEventArgs e)
    {
        writeAllText(EditNoteBox.Text);
    }
    private void EncryptOnClick(object? sender, RoutedEventArgs e)
    {
        if (_encryptionService.EncryptFile(PasswordBlock.Text, _filePath, EditNoteBox.Text))
        {
            LoadFileContent();
            EncryptButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }
    }
    private void DecryptOnClick(object? sender, RoutedEventArgs e)
    {
        var (success, decryptedText) = _encryptionService.DecryptFile(PasswordBlock.Text, _filePath);

        if (success)
        {
            EditNoteBox.Text = decryptedText;
            EncryptButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
        }
    }
}