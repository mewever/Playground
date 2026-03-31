using LinkedList;

LinkedList<string, Person> linkedList = new LinkedList<string, Person>();

Person person = new Person("John Smith", new DateTime(2000, 1, 1), "Software Engineer");
linkedList.Add(person.Name, person);
person = new Person("Jane Smith", new DateTime(1998, 1, 1), "Business Consultant");
linkedList.Add(person.Name, person);
person = new Person("Andrew Andersen", new DateTime(2006, 1, 1), "Sales Representantive");
linkedList.Add(person.Name, person);
person = new Person("Xavier Washington", new DateTime(1990, 1, 1), "Independent Business Owner");
linkedList.Add(person.Name, person);

person = linkedList.Get("Jane Smith") ?? new Person("", DateTime.MinValue, "");
Console.WriteLine("Retrieved person with key 'Jane Smith':");
Console.WriteLine($"Name: {person.Name}, Date of Birth: {person.DateOfBirth}, Occupation: {person.Occupation}");

Console.WriteLine();
Console.WriteLine("List as array:");
Person[] people = linkedList.ToArray();
foreach (var item in people)
{
    Console.WriteLine($"Name: {item.Name}, Date of Birth: {item.DateOfBirth}, Occupation: {item.Occupation}");
}


Console.WriteLine();
Console.WriteLine("List as array:");
Dictionary<string, Person> dictionary = linkedList.ToDictionary();
foreach (var item in dictionary)
{
    Console.WriteLine($"[{item.Key}] Name: {item.Value.Name}, Date of Birth: {item.Value.DateOfBirth}, Occupation: {item.Value.Occupation}");
}

class Person
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Occupation { get; set; }

    public Person(string name, DateTime dateOfBirth, string occupation)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Occupation = occupation;
    }
}