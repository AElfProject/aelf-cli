using System;
using Volo.Abp.Domain.Entities;

namespace CompanyName.ProjectName.Demo;

public class DemoBase : Entity<Guid>
{
   public DateTime Stamp { get; set; }
}