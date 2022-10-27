// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using Newtonsoft.Json;






string path = @"/Users/User/Projects/TodoConsoleProject/TodoConsoleProject/todoList.json";

//try
//{
//    // Create the file, or overwrite if the file exists.
//    using (FileStream fs = File.Create(path))
//    {
//        byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
//        // Add some information to the file.
//        fs.Write(info, 0, info.Length);
//    }

//    // Open the stream and read it back.
//    using (StreamReader sr = File.OpenText(path))
//    {
//        string s = "";
//        while ((s = sr.ReadLine()) != null)
//        {
//            Console.WriteLine(s);
//        }
//    }
//}

//catch (Exception ex)
//{
//    Console.WriteLine(ex.ToString());
//}


//string fileName = System.IO.Path.GetRandomFileName();

//// This example uses a random string for the name, but you also can specify
//// a particular name.
////string fileName = "MyNewFile.txt";

//// Use Combine again to add the file name to the path.
//string pathString = System.IO.Path.Combine("/Users/User/Projects/TodoConsoleProject/TodoConsoleProject", fileName);

//// Verify the path that you have constructed.
//Console.WriteLine("Path to my file: {0}\n", pathString);

////var logFile = File.ReadAllLines(fileName);
////var logList = new List<string>(logFile);

//if (!System.IO.File.Exists(pathString))
//{
//    using (System.IO.FileStream fs = System.IO.File.Create(pathString))
//    {
//        for (byte i = 0; i < 100; i++)
//        {
//            fs.WriteByte(i);
//        }
//    }
//}
//else
//{
//    Console.WriteLine("File \"{0}\" already exists.", fileName);
//    return;
//}



List<Todo> todos = new List<Todo>();


int todoCount = todos.Count;

int count = 0;
if (new FileInfo(path).Length == 0)
{
    Console.WriteLine("You have No Task... Add Some Tasks...");
}
else
{
    StreamReader r = new StreamReader(path);
    string json = r.ReadToEnd();
    List<Todo> fileList = JsonConvert.DeserializeObject<List<Todo>>(json);

    fileList.ForEach(todo =>
    {
        todos.Add(todo);
        if (todo.IsDone == true)
        {
            count = count + 1;
        }
        else
        {
            count = count;
        }

    });
    todoCount = todos.Count();
}



while (true)
{
    Console.WriteLine(
    $">> You have {todoCount} tasks todo and {count} tasks are done!\n" +
    ">> Pick an option:\n" +
    ">> (1) Show Task List\n" +
    ">> (2) Add New Task\n" +
    ">> (3) Edit Task (update, mark as done, remove\n" +
    ">> (4) Save and Quit\n" +
    ">>>>");

    string option = Console.ReadLine();
    if (QuitFunction(option))
    {
        SavingTodosList(todos, path);
        break;
    }
    else if (option == "1")
    {
        ShowTodosList(todos);
    }
    else if (option == "2")
    {
        AddToTodosList(todos);
    }
    else if (option == "3")
    {
        Console.WriteLine("Enter ID For The Task...");
        string updateId = Console.ReadLine();
        UpdateTodosList(todos, updateId);
    }
}


static void ShowTodosList(List<Todo> todos)
{
    var todosOrderById = todos.OrderBy(todo => todo.Id).ToList();
    var todosOrderByTitle = todos.OrderBy(todo => todo.Title).ToList();
    var todosOrderByDate = todos.OrderBy(todo => DateTime.Parse(todo.Date)).ToList();

    Console.WriteLine("Order By ID");
    Console.WriteLine("ID".PadRight(10) + "Title".PadRight(50) + "Date".PadRight(30) + "Complete");
    foreach (Todo todo in todosOrderById)
    {
        Console.WriteLine(todo.Id.ToString().PadRight(10) + todo.Title.PadRight(50) + todo.Date.PadRight(30) + todo.IsDone.ToString());
    }

    Console.WriteLine("Order By Title");
    Console.WriteLine("ID".PadRight(10) + "Title".PadRight(50) + "Date".PadRight(30) + "Complete");
    foreach (Todo todo in todosOrderByTitle)
    {
        Console.WriteLine(todo.Id.ToString().PadRight(10) + todo.Title.PadRight(50) + todo.Date.PadRight(30) + todo.IsDone.ToString());
    }


    Console.WriteLine("Order By Date");
    Console.WriteLine("ID".PadRight(10) + "Title".PadRight(50) + "Date".PadRight(30) + "Complete");
    foreach (Todo todo in todosOrderByDate)
    {
        Console.WriteLine(todo.Id.ToString().PadRight(10) + todo.Title.PadRight(50) + todo.Date.PadRight(30) + todo.IsDone.ToString());
    }
}

static List<Todo> AddToTodosList(List<Todo> todos)
{
    bool idEntering = false;
    bool titleEntering = false;
    bool dateEntering = false;
    bool isDoneEntering = false;



    string id = "";
    string title = "";
    string date = "";
    bool isDone = true;

    Console.Write("Add A New ID : ");
    while (true)
    {
        id = Console.ReadLine();
        if (CheckingId(todos, id))
        {
            Console.WriteLine("ID Is Already Exist");

        }
        else
        {
            idEntering = IsNumber(id);
            if (idEntering)
            {
                Console.Write("Add Task Title : ");
                title = Console.ReadLine();
                titleEntering = IsEmpty(title);
            }
            if (titleEntering)
            {
                Console.Write("Add Date : ");

                date = Console.ReadLine();
                if (IsValidDate(date))
                {
                    dateEntering = true;
                }
                else
                {
                    Console.WriteLine("Please Enter Valid Date...");
                }

            }
            if (dateEntering)
            {
                Console.Write("If The Task is Complete : Y / N ?");
                string complete = Console.ReadLine();
                if (complete.ToLower().Trim() == "y" || complete.ToLower().Trim() == "n")
                {
                    isDone = CheckingIsDone(complete);
                    isDoneEntering = true;
                }
                else
                {
                    Console.WriteLine("Error.. Entering Should Be Y / N ");
                }

            }
            if (isDoneEntering)
            {
                var todo = new Todo(int.Parse(id), title, date, isDone);
                todos.Add(todo);
                break;
            }
        }

    }
    return todos;
}

static List<Todo> UpdateTodosList(List<Todo> todos, string id)
{
    Console.WriteLine(
    ">> (1) Change Title Task \n" +
    ">> (2) Change Date Task \n" +
    ">> (3) Mark Task As Complete \n" +
    ">> (4) Remove The Task");

    string change = Console.ReadLine().Trim();
    if (CheckingId(todos, id))
    {
        var t = todos.Find(todo => todo.Id == int.Parse(id));
        int index = todos.IndexOf(t);

        if (change == "1")
        {
            Console.WriteLine("Enter New Title");
            string newTitle = Console.ReadLine();
            t.Title = newTitle;
        }
        else if (change == "2")
        {
            Console.WriteLine("Enter A New Date : MM/DD/YYYY");
            string newDate = Console.ReadLine();
            if (IsValidDate(newDate))
            {
                t.Date = newDate;
            }
            else
            {
                Console.WriteLine("Please Enter Valid Date...");
            }

        }
        else if (change == "3")
        {
            Console.WriteLine("Task Is Done ? Y / N ");
            string isDone = Console.ReadLine();
            if (isDone.ToLower().Trim() == "y" || isDone.ToLower().Trim() == "n")
            {
                t.IsDone = CheckingIsDone(isDone);
            }
            else
            {
                Console.WriteLine("Error.. Entering Should Be Y / N ");
            }

        }
        else if (change == "4")
        {
            todos.RemoveAt(index);
        }
        else
        {
            Console.WriteLine("Error ...Choose A Right....");
        }
    }
    return todos;
}
static void SavingTodosList(List<Todo> todos, string path)
{
    string json = System.Text.Json.JsonSerializer.Serialize(todos);
    File.WriteAllText(path, json);

}

static bool CheckingId(List<Todo> todos, string id)
{

    List<int> ids = new List<int>();
    foreach (Todo todo in todos)
    {
        ids.Add(todo.Id);
    }
    if (IsEmpty(id))
    {
        return ids.Contains(int.Parse(id));
    }
    else { return false; }

}


static bool CheckingTitle(string title)
{
    if (IsEmpty(title))
    {
        Console.WriteLine("Title Field Is Empty!!");
        return false;
    }
    else
    {
        return true;
    }
}

static bool IsValidDate(string input)
{
    DateTime tempObject; ;
    return DateTime.TryParse(input, out tempObject);
}


static bool CheckingIsDone(string isDone)
{
    if (isDone.ToLower().Trim() == "y")
    {
        return true;
    }
    else
    {
        return false;
    }
}


static bool IsNumber(string id)
{
    if (id.All(char.IsDigit))
    {
        Console.WriteLine("It Is A Number");
        return true;
    }
    else
    {
        Console.WriteLine("It Is NOOOT Number!!!!");
        return false;
    }
}

static bool IsEmpty(string input)
{
    if (String.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Error... Empty Field...");
        return false;
    }
    return true;

}


static bool QuitFunction(string input)
{
    if (input.ToLower().Trim() == "4")
    {
        return true;
    }
    else
    {
        return false;
    }
}




Console.ReadLine();

class Todo
{

    public Todo() { }
    public Todo(int id, string title, string date, bool isDone)
    {
        Id = id;
        Title = title;
        Date = date;
        IsDone = isDone;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Date { get; set; }
    public bool IsDone { get; set; }
}