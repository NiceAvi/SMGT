using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ResultViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Subject is Required")]
        [Display(Name = "Subject")]
        public Nullable<int> SubjectId { get; set; }

        [Required(ErrorMessage = "Teacher is Required")]
        [Display(Name = "Teacher")]
        public Nullable<int> TeacherId { get; set; }


        [Required(ErrorMessage = "Batch date is Required")]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage = "Batch time is Required")]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Time { get; set; }
        public List<BatchStudent> BatchStudents { get; set; }

    }
}