using CompanyName.ProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CompanyName.ProjectName.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ProjectNamePageModel : AbpPageModel
{
    protected ProjectNamePageModel()
    {
        LocalizationResourceType = typeof(ProjectNameResource);
    }
}
