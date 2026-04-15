using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class UploadFileCommand : IRequest<Result<List<Guid>>>
{
    public List<IFormFile> Files { get; set; }
    public long EntityId { get; set; }
    public string EntityName { get; set; } //Project
}
