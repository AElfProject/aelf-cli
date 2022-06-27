using System;
using System.Net;
using System.Net.Http;
using AElf.Cli.Building.File;
using AElf.Cli.Building.Utils;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepDownloadOrReadCachedTemplateFilesZip: ITemplateBuildStep
{
    public void Execute(TemplateBuildContext context)
    {
        context.TemplateZipContent = GetFiles(context);
    }

    private byte[] GetFiles(TemplateBuildContext templateBuildContext)
    {
        FileHelper.CreateIfNotExists(AElfCliPaths.TemplateCache);
        FileHelper.CreateIfNotExists(templateBuildContext.CachedLocation);

        if (FileHelper.CheckIfExistsOrNot(templateBuildContext.CachedFileName))
        {
            return FileHelper.ReadFile(templateBuildContext.CachedFileName);
        }
        else
        {
            var fileBytes = DownloadFiles(templateBuildContext.DownloadUrl);
            FileHelper.WriteFile(templateBuildContext.CachedFileName, fileBytes);
            return fileBytes;
        }
    }

    private byte[] DownloadFiles(string downloadUrl)
    {
        HttpResponseMessage responseMessage = null;

        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(15);

            responseMessage = client.GetAsync(downloadUrl).Result;

            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new AElfCliUsageException(
                    $"The downloading got issue. \r\ndownload url: {downloadUrl}. \r\nresponse: {responseMessage}"
                );
            }

            var resultAsBytes = responseMessage.Content.ReadAsByteArrayAsync().Result;
            
            responseMessage.Dispose();

            return resultAsBytes;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured while downloading source-code from {0} : {1}{2}{3}", downloadUrl,
                responseMessage, Environment.NewLine, ex.Message);
            throw;
        }
    }
}