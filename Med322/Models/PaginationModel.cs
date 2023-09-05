namespace Med322.Models
{
    public class PaginationModel<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalData { get; set; }

        public PaginationModel(List<T> data)
        {
            //PageIndex = pageIndex;
            //TotalPages = (int)Math.Ceiling((double)dataCount / (double)pageSize);
            //TotalData = dataCount;

            this.AddRange(data);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginationModel<T> CreateAsync(List<T> dataSource)
        {
            int dataCount = dataSource.Count;
            List<T> data = dataSource;

            return new PaginationModel<T>(data);

        }
    }
}
