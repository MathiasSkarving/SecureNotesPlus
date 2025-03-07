using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.IO;

namespace SecureNotesPlus;

public partial class NewFileWindow : Window
{
    private string path;
    public NewFileWindow()
    {
        InitializeComponent();
        path = @"..\\..\\..\\Notes\";
    }
    private void CreateNoteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        using FileStream fileStream = File.Create(path + NameFileBox.Text + ".txt");
        WindowLocator.MainWindow.notes.AddNote(new Note(NameFileBox.Text + ".txt"));
        WindowLocator.MainWindow.UpdateNoteList();
    }
    
}