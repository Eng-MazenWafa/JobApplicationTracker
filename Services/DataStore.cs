using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    public class DataStore<T> where T : class, IEntity
    {
        private readonly List<T> _items=new List<T>();

        public bool Add(T item)
        {
            if (item == null)
            {
                return false;
            }

            if (GetByID(item.Id) != null)
            {
                return false;
            }

            
            _items.Add(item);
            return true;
        }

        public T? GetByID(int id)
        {
            foreach(T item in _items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public bool Update(T item)
        {
            if (item == null)
            {
                return false;
            }

            for(int i=0; i < _items.Count; i++)
            {
                if (_items[i].Id== item.Id)
                {
                    _items[i] = item;
                    return true;
                }

            }
            return false;
        }

        public bool Remove(int id)
        {
            for(int i=0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    _items.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public List<T> GetAll()
        {
             List<T> copy = new List<T>(_items);


             return copy;
        }    

        public int GetNextId()
        {
            int maxID = 0;
            foreach(T item in _items)
            {
                if (item.Id > maxID)
                {
                    maxID = item.Id;
                }
            }
            return maxID + 1;


        }






    }

}