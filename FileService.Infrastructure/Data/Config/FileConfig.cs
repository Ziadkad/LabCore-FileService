using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = FileService.Domain.File.File;

namespace FileService.Infrastructure.Data.Config;

public class FileConfig: IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.FileType).IsRequired().HasMaxLength(10);
        builder.Property(e => e.Path).IsRequired();
        builder.Property(e => e.IsAccessible).IsRequired();
        builder.Property(e => e.ProjectId).IsRequired();
    }
}