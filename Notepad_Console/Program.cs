using System;
using System.Collections.Generic;
using System.Threading;

class Notepad_Program
{
	static void Main()
	{
		Notepad notepad = new Notepad();
		notepad.Start();
		while (true)
		{
			Console.WriteLine("(1)Add\n(2)Delete\n(3)List\n(4)TrashBin\n(5)Quit\n");
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
					case 4:
						{
							notepad.Trashbin();
							break;
						}
					case 5:
						{
							return;
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
	private  List<Note> Notes = new List<Note>();
	private  List<Note> Notes_Deleted = new List<Note>();
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
		int select_to_delete = ListAll(Notes, true, "Enter id of note to delete or type \"reset\" to delete all. (Type \"back\" to get back)","reset");
		if (select_to_delete >= 0)
		{
			Notes_Deleted.Add(Notes[select_to_delete]);
			Notes.RemoveAt(select_to_delete);
			SaveNotes();
		}
		else if (select_to_delete == -2)
		{
			for (int i = Notes.Count - 1; i >= 0; i--)
			{
				Notes_Deleted.Add(Notes[i]);
				Notes.RemoveAt(i);
			}
			SaveNotes();
		}
	}
	public void List()
	{
		int select_to_list = ListAll(Notes, true,"Enter id to view. (Type \"back\" to get back)");
		if (select_to_list >= 0)
		{
			Console.WriteLine($"Title:{Notes[select_to_list].title}  Date:{Notes[select_to_list].date.ToString("g")}\n{Notes[select_to_list].content}");
		}
		else { }
	}

	public void Trashbin()
	{
		ListAll(Notes_Deleted, false,"");
		Console.WriteLine("(1)Delete Permanently\n(2)Recover\n(3)Back");
		while (true)
		{
			int Trash_choice = Getint(1,4);
			if (Trash_choice == 1)
			{
				Console.WriteLine("Enter id to delete permanently");
				int delete_permanently = Getint(0, Notes_Deleted.Count);
				if (delete_permanently >= 0 && delete_permanently < Notes_Deleted.Count)
				{
					Notes_Deleted.RemoveAt(delete_permanently);
					; break;
				}
				else { break; }
			}
			else if (Trash_choice == 2)
			{
				Console.WriteLine("Enter id of note to recover");
				int recover = Getint(0, Notes_Deleted.Count);
				if (recover >= 0 && recover < Notes_Deleted.Count)
				{
					Notes.Add(Notes_Deleted[recover]);
					Notes_Deleted.RemoveAt(recover);
					break;
				}
				else { break; }
			}
			else if (Trash_choice == -1)
			{
				break;
			}
			else if (Trash_choice == 3)
			{
				break;
			}
		}
		SaveNotes();
	}

	private int ListAll(List<Note> List,bool choose,string show = "Enter a number",string special = "giveto")
	{
		Console.WriteLine("ID  Title");
		Console.WriteLine("--  -----");
		for (int i = 0; i < List.Count; i++)
		{
			Console.WriteLine($"{i.ToString("d2")} {List[i].date.ToString("g")} {List[i].title}");
		}
		if (choose)
		{
			Console.WriteLine($"{show}");
			while (true)
			{
				string input = Console.ReadLine();
				if (int.TryParse(input, out int result))
				{
					if (result >= 0 && result < List.Count)
					{
						int list_choice = result;
						return list_choice;
					}
					else
					{
						Console.WriteLine("Index out of range.");
					}
				}
				else if (input == "back")
				{
					return -1;
				}
				else if (input == special && special !="giveto")
				{
					return -2;
				}
				else
				{
					Console.WriteLine("Enter a valid number or quit by entering \"back\"");
				}

			}
		}
		else 
		{ 
			return 1; 
		}
	}
	private void SaveNotes()
	{
		using (StreamWriter writer = new StreamWriter(FilePath))
		{

			foreach (var note in Notes)
			{
				writer.WriteLine($"{note.date.ToString("o")}|{note.title}|{note.content}|true");
			}
			foreach (var note in Notes_Deleted)
			{
				writer.WriteLine($"{note.date.ToString("o")}|{note.title}|{note.content}|false");
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
				if (a[3] == "true")
				{
					Notes.Add(note);
				}
				else
				{
					Notes_Deleted.Add(note);
				}
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
	public static int Getint(int min, int max)
	{
		while (true)
		{
			string a = Console.ReadLine();
			if (int.TryParse(a, out int b))
			{
				if (min <= b && b < max)
				{
					return b;
				}
				else 
				{
					Console.WriteLine("Number out of range. Try Again...");
					continue;
				}
			}
			else if (a == "back")
			{
				return -1;
			}
			else
			{
				Console.WriteLine("Enter a valid value (Integer) or (Type \"back\" to get back)");
				continue;
			}
		}

	}
}