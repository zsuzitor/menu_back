﻿
using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.CodeReviewApp.DAL.Domain
{
    public class Project: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<ProjectUser> Users { get; set; }
        //public List<string> LocalUsers { get; set; }
        public List<TaskReview> Tasks { get; set; }

        public Project()
        {
            Users = new List<ProjectUser>();
            Tasks = new List<TaskReview>();
        }

    }
}