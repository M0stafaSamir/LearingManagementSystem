using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;
using LMS.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.ViewModel.Inst
{
    public class CreateCourseViewModel
    {
 
        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(10, 5000)]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string InstructorId { get; set; }



    }
}
