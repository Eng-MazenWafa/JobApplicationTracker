using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JobApplicationTracker.Practice
{
    public static class GenericsPractice
    {

         public static void swap<T>( ref T value1,ref T value2)
        {
           T temp = value1;
            value1 = value2;
            value2 = temp;
        }


        class Any<T>
        {
          public List<T> _Items { get; set; }

            public Any()
            {
                _Items = new List<T>();
            }

            public void AddItem(T item)
            {
                _Items.Add(item);
            }

            public void DisplayItems()
            {
                foreach (var item in _Items)
                {
                    Console.WriteLine(item);
                }
            }


        }


        public static void RunAllGenericsPractices()
        {

            Console.WriteLine("swap-------------");
            int a = 5;
            int b = 10;
            string c= "Hello";
            string d = "World";
            swap(ref c, ref d);
            Console.WriteLine(c+d);
            Console.WriteLine(a+b);


            Console.WriteLine("Any-------------");
            var anyList1 = new Any<string>();
            anyList1.AddItem("Item 1");
            anyList1.AddItem("Item 2");
            anyList1.DisplayItems();
            var anyList2 = new Any<int>();
            anyList2.AddItem(1);
            anyList2.AddItem(2);
            anyList2.DisplayItems();






        }
    }
}