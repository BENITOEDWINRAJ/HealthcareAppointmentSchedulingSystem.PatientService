using PatientService.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Application.Handlers.Interfaces
{
    public interface ILoginHandler
    {        
        Task<string> Handle(LoginCommand query);
    }
}
