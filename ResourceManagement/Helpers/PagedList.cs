using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

    public class OrderedPageList<T> : PagedList<T>
    {
        public OrderedPageList(List<T> items, int count, int pageNumber, int pageSize) 
            : base(items, count, pageNumber, pageSize)
        {
        }

        private void Order()
        {
            //this.Order
        }

        internal int SetId()
        {
            return 0;
        }

        internal void Shuffle<T>()
        {
            Random random = new Random();
            int n = this.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                //T value = this[k];
                //list[k] = list[n];
                //list[n] = value;
            }

        }
    }

    public class Context<T>
    {
        private Dictionary<int, T> _dictionaryContext;
        public string Name { get; private set; }

        public Context(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _dictionaryContext = new Dictionary<int, T>();
        }

        public int Count
        {
            get { return _dictionaryContext.Count(); }
        }
        public Dictionary<int, T>.KeyCollection Keys
        {
            get { return _dictionaryContext.Keys; }
        }
        public Dictionary<int, T>.ValueCollection Values
        {
            get { return _dictionaryContext.Values; }
        }


        public void Add(int key, T value)
        {
            _dictionaryContext.Add(key, value);
        }

        public bool Remove(int key)
        {
            return _dictionaryContext.Remove(key);
        }

        public void Insert(int key, T value)
        {
            _dictionaryContext.Add(key, value);
        }

    }
    
}
