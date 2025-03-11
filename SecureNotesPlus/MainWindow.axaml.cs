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
        try
        {
            WindowLocator.EditWindow = new EditWindow(NotesBox.SelectedItem as Note);
            WindowLocator.EditWindow.Show();
        }
        catch (NullReferenceException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    private void NewButton_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowLocator.NewFileWindow = new NewFileWindow(notes.getNotes());
        WindowLocator.NewFileWindow.Show();
    }

    private void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            notes.DeleteNote(NotesBox.SelectedItem as Note);
        }
        catch(NullReferenceException nRE)
        {
        }
    }

    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    public void SearchNotes()
    {
        if (SearchBar.Text == "" || SearchBar.Text == null)
        {
            NotesBox.Items.Clear();

            foreach (Note note in notes.getNotes())
            {
                NotesBox.Items.Add(note);
            }   
        }
        else
        {
            NotesBox.Items.Clear();

            foreach (Note note in notes.getNotes())
            {
                if (note.getFileName().Contains(SearchBar.Text))
                {
                    NotesBox.Items.Add(note);
                }
            }   
        }
    }

    public void UpdateNoteList()
    {
        NotesBox.Items.Clear();
        foreach (Note note in notes.getNotes())
        {
            NotesBox.Items.Add(note);
        }
    }

    private void SearchBar_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchNotes();
    }
}