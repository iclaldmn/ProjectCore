using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FileMinio;

public class FileReference
{
    public Guid Id { get; set; }

    public Guid FileId { get; set; }
    public FileEntity File { get; set; }

    public long EntityId { get; set; }
    public string EntityName { get; set; }
    // "Project", "User", "Task" vs

    public bool Silindi { get; set; }
    public DateTime CreatedAt { get; set; }
}