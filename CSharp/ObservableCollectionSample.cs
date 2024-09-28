using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Utils
{
    public class ObservableCollectionSample
    {
        public void Test()
        {
            // Create an ObservableCollection of strings
            ObservableCollection<string> fruits = new ObservableCollection<string>();

            // Subscribe to the CollectionChanged event
            fruits.CollectionChanged += Fruits_CollectionChanged;

            // Add items to the collection
            fruits.Add("Apple");
            fruits.Add("Banana");
            fruits.Add("Orange");

            // Remove an item from the collection
            fruits.Remove("Banana");

            // Replace an item
            fruits[0] = "Mango";
        }

        // Event handler for CollectionChanged event
        private void Fruits_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Console.WriteLine($ "Item added: {e.NewItems[0]}");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Console.WriteLine($ "Item removed: {e.OldItems[0]}");
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Console.WriteLine($ "Item replaced: {e.OldItems[0]} with {e.NewItems[0]}");
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Console.WriteLine("Collection reset");
                    break;
            }
        }
    }
}