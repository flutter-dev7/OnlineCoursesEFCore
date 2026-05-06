using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Infrastructure.Configurations;

public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Один студент может иметь много записей прогресса, но для конкретного урока - одну
        builder.HasIndex(x => new { x.StudentId, x.LessonId }).IsUnique();

        builder.HasOne(x => x.Lesson)
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}