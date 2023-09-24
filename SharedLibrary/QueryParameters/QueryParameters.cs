using System.Collections.Generic;
using Commons.Models;

namespace Commons.Types
{
    public abstract class QueryParameters<T> where T : IndexedEntity
    {
        public int PageIndex { get; set; } = 0;
        public abstract void ExecuteFilter(ref IEnumerable<T> data);
        public abstract void ExecuteSort(ref IEnumerable<T> data);
        public abstract void ExecutePaginate(ref IEnumerable<T> data);

        public IEnumerable<T> Execute(IEnumerable<T> data)
        {
            var result = data;
            
            ExecuteFilter(ref result);
            ExecuteSort(ref result);
            ExecutePaginate(ref result);
            
            return result;
        }
    }
}