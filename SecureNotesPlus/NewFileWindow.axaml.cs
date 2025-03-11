using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.IO;
using System.Collections.Generic;

namespace SecureNotesPlus;

public partial class NewFileWindow : Window
{
    string exeDir = AppDomain.CurrentDomain.BaseDirectory;
    private string path;
    private List<Note> notes;
    public NewFileWindow(List<Note> notes)
    {
        InitializeComponent();
        this.notes = notes;
        path = exeDir+@"\Notes\";
    }
    private void CreateNoteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        bool success = true;
        foreach (Note note in notes)
        {
            if (note.getFileName() == NameFileBox.Text + ".txt")
            {
                success = false;
                break;
            }
            else
            {
                success = true;
            }
        }

        if (success)
        {
            using FileStream fileStream = File.Create(path + NameFileBox.Text + ".txt");
            WindowLocator.MainWindow.notes.AddNote(new Note(NameFileBox.Text + ".txt"));
            WindowLocator.MainWindow.UpdateNoteList();
        }
    }
    
}