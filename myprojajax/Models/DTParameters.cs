namespace myprojajax.Models
{
    public class DTParameters
    {
        public int Draw { get; set; }
        public List<DTOrder> Order { get; set; }
        public DTSearch Search { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }

    public class DTOrder
    {
        public int Column { get; set; }
        public DTOrderDir Dir { get; set; }
    }

    public enum DTOrderDir
    {
        ASC,
        DESC
    }

    public class DTSearch
    {
        public string Value { get; set; }
    }

    // DTResult is used to format the response sent back to the DataTable
    public class DTResult<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}
