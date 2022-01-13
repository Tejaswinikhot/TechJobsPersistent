using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {

        [Required(ErrorMessage = "Job Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employer Id is required")]
        public int EmployerId { get; set; }

        //[Required(ErrorMessage = "Employer is required")]
        public List<SelectListItem> Employers { get; set; }


        public List<Skill> Skills { get; set; }

        public string[] selectedSkills { get; set; }

        public AddJobViewModel(List<Employer> employers, List<Skill> skills)
        {
            Employers = new List<SelectListItem>();

            foreach (var employer in employers)
            {
                Employers.Add(
                    new SelectListItem
                    {
                        Value = employer.Id.ToString(),
                        Text = employer.Name
                    }
                );
            }

            this.Skills = skills;
        }
        public AddJobViewModel() { }
    }
}
