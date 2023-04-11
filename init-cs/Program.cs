// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using (StreamWriter sw = new StreamWriter("test.txt"))
{
    sw.WriteLine("mega, test, xd");
}

Console.WriteLine("Napsal jsem ti to souboru 'text.txt': 'mega, text, xd'");

Console.ReadKey();