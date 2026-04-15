using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

//Tekli Silme İşlemi
public class DeleteFileCommand : IRequest<Result<bool>>
{
    public Guid FileReferenceId { get; set; }
}