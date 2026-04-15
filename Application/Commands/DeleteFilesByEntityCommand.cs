using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

//Çoklu Dosya Silme İşlemi
public class DeleteFilesByEntityCommand : IRequest<Result<bool>>
{
    public long EntityId { get; set; }
    public string EntityName { get; set; }
}