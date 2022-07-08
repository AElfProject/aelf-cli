using System;
using Volo.Abp.Domain.Repositories;

namespace CompanyName.ProjectName.Demo
{
    public interface IDemoRepository : IRepository<Demo, Guid>
    {
        
    }
}