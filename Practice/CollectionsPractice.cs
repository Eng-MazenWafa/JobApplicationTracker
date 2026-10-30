using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JobApplicationTracker.Practice
{
    public static class CollectionsPractice
    {


        
        public  static void RunAllPractices()
        {
            
                Console.WriteLine("Running Collections Practice...");
                Console.WriteLine(@"Running List Practice...
...
");
                RunListPractice();


        }


        public   static void RunListPractice()
        {
            Console.WriteLine("Running List Practice...");


            void displayNames(List<string> names)
            {
                foreach (var name in names)
                {
                    Console.WriteLine(name);
                }
            }
            List<string> names = new List<string> { "mazen", "Adel", "Ahmed" };

            //add
            Console.WriteLine("add---------------");
            names.Add("Ali");
            displayNames(names);

            //remove
            Console.WriteLine("remove---------------"); 
            names.Remove("Adel");
            displayNames(names);

            //update    
            Console.WriteLine("update---------------");
            names[0] = "Mazen Updated";

            //insert at index
            Console.WriteLine("insert---------------");
            names.Insert(1, "New Name");
            displayNames(names);


            //copy
            Console.WriteLine("copy1---------------");
            var copynames1=new List<string>(names);
            displayNames(copynames1);
            Console.WriteLine("copy2---------------");

          
           

        }




    }
}