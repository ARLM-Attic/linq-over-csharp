using System;

partial class Customer
{
  string name; 
  public string Name
  {
    get { return name; } 
    set
    {
      OnNameChanging(value); 
      name = value; 
      OnNameChanged();
    }
  } 

  partial void OnNameChanging(string newName);   
  partial void OnNameChanged(); 
}

partial class Customer 
{
  partial void OnNameChanging(string newName) 
  { 
    Console.WriteLine("Changing " + name + " to " + newName);
  }

  partial void OnNameChanged() 
  { 
    Console.WriteLine("Changed to " + name); 
  } 
}
