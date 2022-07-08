using CompanyName.ProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CompanyName.ProjectName.WebApi.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ProjectNameController : AbpControllerBase
{
    protected ProjectNameController()
    {
        LocalizationResource = typeof(ProjectNameResource);
    }
}
