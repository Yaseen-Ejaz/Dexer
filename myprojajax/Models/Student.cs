using System.ComponentModel;

namespace myprojajax.Models
{
    public class Student
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string? firstName { get; set; }
        public string? lastName { get; set; }

        public string? indexId { get; set; }

    }
}
