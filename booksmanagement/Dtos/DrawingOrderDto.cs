﻿using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class DrawingOrderDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Purpose { get; set; }
        public string Location { get; set; }
        public string JobNumber { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
        public CarPart CarPart { get; set; }
        public int? CarPartId { get; set; }
        public CarPartComponent CarPartComponent { get; set; }
        public int? CarPartComponentId { get; set; }
        public ApplicationUser Applicant { get; set; }
        public string ApplicantId { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        public ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public bool DrawingRejected { get; set; }
        public DateTime? DrawingRejectedDate { get; set; }
        public string RejectionComments { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}