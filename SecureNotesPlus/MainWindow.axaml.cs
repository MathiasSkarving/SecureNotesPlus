using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace SecureNotesPlus;

public partial class MainWindow : Window
{
    public Notes notes = new Notes();
    public MainWindow()
    {
        InitializeComponent();
        WindowLocator.MainWindow = this;
        notes.syncNotes();
        UpdateNoteList();
    }
    private void EditButton_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowLocator.EditWindow = new EditWindow(NotesBox.SelectedItem as Note, NotesBox.SelectedItem.ToString());
        WindowLocator.EditWindow.Show();
    }

    private void NewButton_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowLocator.NewFileWindow = new NewFileWindow();
        WindowLocator.NewFileWindow.Show();
    }

    private void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            notes.DeleteNote(NotesBox.SelectedItem as Note, NotesBox.SelectedItem.ToString());
        }
        catch(NullReferenceException nRE)
        {
            throw new NullReferenceException(nRE.Message);
        }
    }

    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    public void UpdateNoteList()
    {
        NotesBox.Items.Clear();
        foreach (Note note in notes.getNotes())
        {
            NotesBox.Items.Add(note);
        }
    }
}