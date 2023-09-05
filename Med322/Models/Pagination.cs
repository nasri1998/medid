using Med322.ViewModels;

namespace Med322.Models
{
    public class Pagination<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalData { get; set; }

        public Pagination(List<T> data, int dataCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling((double)dataCount / (double)pageSize);
            TotalData = dataCount;

            this.AddRange(data);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static Pagination<T> CreateAsync(List<T> dataSource, int pageIndex, int pageSize)
        {
            int dataCount = dataSource.Count;
            List<T> data = dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new Pagination<T>(data, dataCount, pageIndex, pageSize);

        }

      
    }
}
