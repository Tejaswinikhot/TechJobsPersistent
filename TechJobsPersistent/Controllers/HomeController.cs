﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();
            AddJobViewModel addEmployerViewModel = new AddJobViewModel(employers, skills);
            return View(addEmployerViewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Employer employer = context.Employers.Find(addJobViewModel.EmployerId);
                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    Employer = employer
                };
                List<JobSkill> jobSkills = new List<JobSkill>();
                foreach (string skill in selectedSkills)
                {
                    JobSkill jobSkill= new JobSkill();
                    jobSkill.SkillId = Int32.Parse(skill);
                    jobSkill.Job = newJob;
                    jobSkills.Add(jobSkill);
                }
                newJob.JobSkills = jobSkills;

                context.Jobs.Add(newJob);
                _ = context.SaveChanges();

                return Redirect("/Home");
            }
            List<Employer> employers = context.Employers.ToList();
            addJobViewModel.Employers = new List<SelectListItem>();
            foreach (var employer in employers)
            {
                addJobViewModel.Employers.Add(
                    new SelectListItem
                    {
                        Value = employer.Id.ToString(),
                        Text = employer.Name
                    }
                );
            }

            List<Skill> skills = context.Skills.ToList();
            addJobViewModel.Skills = skills;
            return View("AddJob", addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
