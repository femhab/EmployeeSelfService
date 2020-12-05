using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.DataContext
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalWorkItem>().HasData(
                new ApprovalWorkItem { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = "Data Seed", Name = "Leave", Description = "Leave Service" },
                new ApprovalWorkItem { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = "Data Seed", Name = "Loan", Description = "Loan Service" },
                new ApprovalWorkItem { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = "Data Seed", Name = "Appraisal", Description = "Appraisal Service" },
                new ApprovalWorkItem { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = "Data Seed", Name = "Transfer", Description = "Transfer Service" },
                new ApprovalWorkItem { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = "Data Seed", Name = "Diciplinary", Description = "Diciplinary Service" }
            );
        }
    }
}
