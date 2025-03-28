using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PT10
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        public SortableBindingList() : base() { }

        public SortableBindingList(IList<T> list) : base(list) { }

        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;
        protected override bool IsSortedCore => sortProperty != null;
        protected override PropertyDescriptor SortPropertyCore => sortProperty;
        protected override ListSortDirection SortDirectionCore => sortDirection;

        public void Sort<K>(string propertyName) where K : IComparable
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName); // pobranie informacji o właściwości
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' is not on type '{typeof(T).Name}'");
            }

            if (property.PropertyType.GetInterface(nameof(IComparable)) == null)
            {
                throw new ArgumentException($"Property '{propertyName}' doesn't implement IComparable interface");
            }

            List<T> sortedList = this.Items.OrderBy(x => (K)property.GetValue(x)).ToList(); // posortowanie listy na podstawie własności właściwości
            this.ClearItems();
            foreach (var item in sortedList)
            {
                this.Add(item); // dodawanie elementów do SortableBindingList
            }
        }

        public List<int> FindCore(string searchTerm) // szukanie wartości atrybutu
        {
            List<int> foundIndexes = new List<int>();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return foundIndexes;

            StringComparison comparison = StringComparison.OrdinalIgnoreCase;

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T))) // iteracja przez wszystkie atrybuty
            {
                for (int i = 0; i < Count; ++i)
                {
                    T item = this[i];
                    object value = prop.GetValue(item);
                    if (value != null && value.ToString().IndexOf(searchTerm, comparison) >= 0) // porównanie
                        foundIndexes.Add(i); // dodanie do listy znalezionych indeksów
                }
            }

            return foundIndexes;
        }
    }
}
