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
                Console.WriteLine(@"Running Dictionary Practice...
...
");
            RunDictionaryPractice();
                Console.WriteLine(@"Running Stack Practice...
...
");
            RunStackPractice();
                Console.WriteLine(@"Running Queue Practice...
...
");
            RunQueuePractice();
                Console.WriteLine(@"Running HashSet Practice...
...
");
            RunHashSetPractice();


        }


        #region list
        public static void RunListPractice()
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
            var copynames1 = new List<string>(names);
            displayNames(copynames1);




        }
        #endregion


        #region dictionary
        public static void RunDictionaryPractice()
        {

            void displayDictionary(Dictionary<int, string> myDictionary)
            {
                foreach (var kvp in myDictionary)
                {
                    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                }
            }
            Dictionary<int, string> myDictionary = new Dictionary<int, string>()
            {
                { 1, "mazen" },
                {2, "Adel" },
                {3, "Ahmed" }
            };

            //add
            Console.WriteLine("add---------------");
            myDictionary.Add(4, "Ali");
            displayDictionary(myDictionary);

            //remove
            Console.WriteLine("remove---------------");
            myDictionary.Remove(2);
            displayDictionary(myDictionary);

            //updaate
            Console.WriteLine("update---------------");
            myDictionary[1] = "Mazen Updated";
            displayDictionary(myDictionary);

            //try get
            myDictionary.TryGetValue(6, out string? value);
            Console.WriteLine($"Value for key 3: {value ?? "Key not found"}");

            //contains
            Console.WriteLine("contains---------------");
            Console.WriteLine(myDictionary.ContainsKey(6));
            Console.WriteLine(myDictionary.ContainsValue("Ali"));

            //ordering
            Console.WriteLine("ordering---------------");
            List<int> sortedKeys = new List<int>(myDictionary.Keys);
            sortedKeys.Sort();
            foreach (var key in sortedKeys)
            {
                Console.WriteLine($"Key: {key}, Value: {myDictionary[key]}");
            }
        }

        #endregion


        #region stack
             public static void RunStackPractice()
        {
            Stack<int> stack = new Stack<int>();

            //push
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            //display
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            //peek
            Console.WriteLine("peeek---------------");
            Console.WriteLine(stack.Peek());

            //pop
            Console.WriteLine("pop---------------------");
            Console.WriteLine(stack.Pop());

        }
        #endregion

        #region queue
        public static void RunQueuePractice()
        {
            Queue<int> queue = new Queue<int>();

            ////enqueu
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);

            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }
            //dequeue
            Console.WriteLine("dequeue---------------");
            Console.WriteLine(queue.Dequeue());


            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }


        }
        #endregion

        #region HAshSet
        public static void RunHashSetPractice()
        {
            void displayHashSet(HashSet<int> myHashSet)
            {
                foreach (var item in myHashSet)
                {
                    Console.WriteLine(item);
                }
            }

            HashSet<int> myHashSet1 = new HashSet<int> { 1, 2, 3, 4, 5 };    
            HashSet<int> myHashSet2 = new HashSet<int> { 4, 5, 6, 7, 8 };
            HashSet<int> myHashSet3 = new HashSet<int> { 4, 5};


            //add 
            Console.WriteLine("add------------------");
            myHashSet1.Add(6);
            displayHashSet(myHashSet1);

            //remove
            Console.WriteLine("remove------------------");
            myHashSet1.Remove(6);
            displayHashSet(myHashSet1);

            //union
            Console.WriteLine("union------------------");
            var unionSet = myHashSet1.Union(myHashSet2);
            // or  myHashSet1.UnionWith(myHashSet2);
            displayHashSet(new HashSet<int>(unionSet));

            //intersection
            Console.WriteLine("intersection------------------");
            var intersectionSet = myHashSet1.Intersect(myHashSet2);
            // or  myHashSet1.IntersectWith(myHashSet2);
            displayHashSet(new HashSet<int>(intersectionSet));

            //difference
            Console.WriteLine("difference------------------");
            var differenceSet = myHashSet1.Except(myHashSet2);
            displayHashSet(new HashSet<int>(differenceSet));

            // issubset
            Console.WriteLine("issubset------------------");
            Console.WriteLine(myHashSet3.IsSubsetOf(myHashSet1)); // True



        }
        #endregion














    }
}