using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FileMinio;

public class FileEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ObjectName { get; set; } // MinIO key
    public string Bucket { get; set; }

    public string ContentType { get; set; }
    public long Size { get; set; }
    public string? UploadedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // ilişki
    public ICollection<FileReference> References { get; set; }
}
