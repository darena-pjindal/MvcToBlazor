using System.ComponentModel.DataAnnotations;

namespace MVCToBlazor
{
    public class MyTask
    {
        [Required]
        public string Title { get; set; }

        public bool Status { get; set; }
    }
}
