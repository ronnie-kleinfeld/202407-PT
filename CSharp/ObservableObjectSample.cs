using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ObservableObjectExample
{
    // ObservableObject base class implementing INotifyPropertyChanged
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to set property value and raise PropertyChanged event
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }

    // Example of a class that uses ObservableObject
    public class Person : ObservableObject
    {
        private string _name;
        private int _age;

        public string Name
        {
            get = > _name;
            set = > SetProperty(ref _name, value);
            }
            public int Age
        {
            get = > _age;
            set = > SetProperty(ref _age, value);
            }
        }

        public class Program
        {
            public
                static void Main(string[] args)
            {
                // Create a new Person object
                var person = new Person { Name = "John", Age = 30 };

                // Subscribe to the PropertyChanged event
                person.PropertyChanged += Person_PropertyChanged;

                // Change properties
                person.Name = "Jane";
                person.Age = 35;

                Console.ReadLine();
            }

            // Event handler for property changes
            private static void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Console.WriteLine($ "Property {e.PropertyName} has changed.");
            }
        }
    }
}