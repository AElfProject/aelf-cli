namespace AElf.Cli.Building.Utils;

public static class AElfCliHttpAddresses
{
    private const string AElfWwwRootPath = "http://localhost:8080";

    public static string TemplateRootAddress => $"{AElfWwwRootPath}/templates";
    
    public static string TemplateDownloadUrl(string templateName, string version)
    {
        //DEBUG: http://localhost:8080/templates/app/latest.zip
        return $"{TemplateRootAddress}/{templateName}/{version}.zip";
    }
}