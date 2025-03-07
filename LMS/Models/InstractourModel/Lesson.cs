using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LMS.Models.StudentModels;

namespace LMS.Models.InstractourModel
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set;}

        [Required]
        public string MediaLink { get; set; }
        public bool IsDeleted { get; set; } = false;

        //added bec its in db but not here
        public int CourseId { get; set; }
        public Course Course { get; set; }



        [ForeignKey("Chapter")]
        public int ChapterID { get; set; }
        public Chapter Chapter { get; set; }
        public List<Note> Notes { get; set; }


    }
}
