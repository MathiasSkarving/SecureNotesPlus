using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using System;

namespace SecureNotesPlus;

public class Notes
{
   private List<Note> noteList = new List<Note>(); 
   string exeDir = AppDomain.CurrentDomain.BaseDirectory;
   private string path;
   
   public Notes()
   {
      path = exeDir+@"\Notes\";
   }

   public void AddNote(Note note)
   {
      noteList.Add(note);
   }

   public void DeleteNote(Note note)
   {
      noteList.Remove(note);
      File.Delete(path + note.getFileName());
      WindowLocator.MainWindow.UpdateNoteList();
   }

   public List<Note> getNotes()
   {
      return noteList;
   }

   public void syncNotes()
   {
      noteList = new List<Note>();
      string[] files = Directory.GetFiles(path);
      foreach (string file in files)
      {
         noteList.Add(new Note(Path.GetFileName(file)));
      }
   }
    
}