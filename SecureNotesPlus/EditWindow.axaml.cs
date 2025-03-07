using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SecureNotesPlus;

public partial class EditWindow : Window
{
    private Note currentNote;
    private string currentFileName = "";
    private bool encrypted;

    public EditWindow(Note noteInput)
    {
        InitializeComponent();
        currentNote = noteInput;
        currentFileName = noteInput.getFileName();

        using (FileStream fileStream = new FileStream(@"..\\..\\..\\Notes\" + currentFileName, FileMode.Open,
                   FileAccess.Read, FileShare.ReadWrite))
        using (StreamReader reader = new StreamReader(fileStream))
        {
            EditNoteBox.Text = reader.ReadToEnd();
        }
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        using (FileStream fileStream = new FileStream(@"..\\..\\..\\Notes\" + currentFileName, FileMode.Create,
                   FileAccess.Write, FileShare.ReadWrite))
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(EditNoteBox.Text);
        }
    }

    private void SaveEncryptButton_OnClick(object? sender, RoutedEventArgs e)
    {
            EncryptionService encryptionService = new EncryptionService();
            encryptionService.EncryptFile(PasswordBlock.Text, currentFileName, EditNoteBox.Text);
            using (FileStream fileStream = new FileStream(@"..\\..\\..\\Notes\" + currentFileName, FileMode.Open,
                       FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                EditNoteBox.Text = reader.ReadToEnd();
            }
    }


    private void DecryptAndSaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!String.IsNullOrWhiteSpace(EditNoteBox.Text))
        {
            EncryptionService encryptionService = new EncryptionService();
            string decryptedText = encryptionService.DecryptFile(PasswordBlock.Text, currentFileName);

            using (FileStream fileStream = new FileStream(@"..\\..\\..\\Notes\" + currentFileName, FileMode.Create,
                       FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(decryptedText);
                EditNoteBox.Text = decryptedText;
            }
        }
    }

    private void DecryptAndShowButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!String.IsNullOrWhiteSpace(EditNoteBox.Text))
        {
            EncryptionService encryptionService = new EncryptionService();
            string decryptedText = encryptionService.DecryptFile(PasswordBlock.Text, currentFileName);

            EditNoteBox.Text = decryptedText;
        }
    }
}