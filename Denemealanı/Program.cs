using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
	static void Main()
	{
		Notepad notepad = new Notepad();
		notepad.Start();
		while (true)
		{
			Console.WriteLine("(1)Add\n(2)Delete\n(3)List\n");
			string choice = Console.ReadLine();
			if (int.TryParse(choice, out int a))
			{
				switch (a)
				{
					case 1:
						{
							notepad.Add();
							break;
						}
					case 2:
						{
							notepad.Delete();
							break;
						}
					case 3:
						{
							notepad.List();
							break;
						}
					default:
						{
							Console.WriteLine("Enter a valid number\n");
							break;
						}
				}
			}
			else 
			{
				Console.WriteLine("Enter a valid number\n");
			}
		}
	}
}
class Note
{
	public string title;
	public string content;
	public DateTime date = DateTime.Now;
}
class Notepad
{
	private string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Text.txt");
	List<Note> Notes = new List<Note>();
	public void Add()
	{
		Note note = new Note();
		Console.Write("Enter title:");
		note.title = Console.ReadLine();
		Console.WriteLine("Enter the content");
		note.content = Console.ReadLine();
		Notes.Add(note);
		SaveNotes();
	}
	public void Delete()
	{
		Console.WriteLine("ID  Title");
		Console.WriteLine("--  -----");
		for (int i = 0; i < Notes.Count; i++)
		{
			Console.WriteLine($"{i.ToString("d2")} {Notes[i].title}");
		}
		Console.Write("Enter the note id to delete. Enter \"reset\" to delete all:");
		while (true)
		{
			string a = Console.ReadLine();
			if (int.TryParse(a, out int index))
			{
				if (index >= 0 && index < Notes.Count)
				{
					Notes.RemoveAt(index);
					Console.WriteLine("Note removed.");
					break;
				}
				else
				{
					Console.WriteLine("Index out of range.");
				}
			}
			else if (a == "reset")
			{
				Notes.Clear();
				Console.WriteLine("All notes moved to trashbin");
				break;
			}
			else
			{
				Console.WriteLine("Enter a valid number");
			}
		}
		SaveNotes();
	}
	public void List()
	{
		Console.WriteLine("ID       Time        Title");
		Console.WriteLine("--  ---------------- -----");
		for (int i = 0; i < Notes.Count; i++)
		{
			Console.WriteLine($"{i.ToString("d2")} {Notes[i].date.ToString("g")} {Notes[i].title}");
		}

		Console.WriteLine("Enter note id to view");
		while (true)
		{
			string input = Console.ReadLine();
			if (int.TryParse(input, out int result))
			{
				if (result >= 0 && result < Notes.Count)
				{
					int list_choice = result;
					Console.WriteLine($"Title:{Notes[list_choice].title} Date:{Notes[list_choice].date.ToString("g")}\n{Notes[list_choice].content}");
					break;
				}
				else
				{
					Console.WriteLine("Index out of range.");
				}
			}
			else if (input == "quit")
			{
				break;
			}
			else
			{
				Console.WriteLine("Enter a valid number or quit by entering \"quit\"");
			}
			
		}
	}

	private void SaveNotes()
	{
		using (StreamWriter writer = new StreamWriter(FilePath))
		{

			foreach (var note in Notes)
			{
				writer.WriteLine($"{note.date.ToString("o")}|{note.title}|{note.content}");
			}
		}

	}

	private void LoadNotes()
	{
		using (StreamReader reader = new StreamReader(FilePath))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				Note note = new Note();
				var a = line.Split("|");
				DateTime myDate = DateTime.Parse(a[0]);
				note.date = myDate;
				note.title = a[1];
				note.content = a[2];
				Notes.Add(note);
			}
		}
	}
	public void Start()
	{
		try
		{
			LoadNotes();
		}
		catch (System.IO.FileNotFoundException)
		{
			SaveNotes();
		}
	}
}