using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IMinioService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType);
    Task DeleteAsync(string objectName);
}