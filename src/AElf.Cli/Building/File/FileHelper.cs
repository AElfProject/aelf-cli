using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace AElf.Cli.Building.File;

public static class FileHelper
{
    public static void ExtractProjectZip(byte[] zipContent, string outputFolder)
    {
        using var templateFileStream = new MemoryStream(zipContent);
        using var zipInputStream = new ZipInputStream(templateFileStream);
        var zipEntry = zipInputStream.GetNextEntry();
        while (zipEntry != null)
        {
            if (string.IsNullOrWhiteSpace(zipEntry.Name))
            {
                zipEntry = zipInputStream.GetNextEntry();
                continue;
            }

            var fullZipToPath = Path.Join(outputFolder, zipEntry.Name);
            var directoryName = Path.GetDirectoryName(fullZipToPath);

            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var fileName = Path.GetFileName(fullZipToPath);
            if (fileName.Length == 0)
            {
                zipEntry = zipInputStream.GetNextEntry();
                continue;
            }

            var buffer = new byte[4096]; // 4K is optimum
            using (var streamWriter = System.IO.File.Create(fullZipToPath))
            {
                StreamUtils.Copy(zipInputStream, streamWriter, buffer);
            }

            zipEntry = zipInputStream.GetNextEntry();
        }
    }

    public static void CreateIfNotExists(string directory)
    {
        if (Directory.Exists(directory)) return;
        if (directory != null) Directory.CreateDirectory(directory);
    }

    public static bool CheckIfExistsOrNot(string file)
    {
        return System.IO.File.Exists(file);
    }

    public static void WriteFile(string filePath, byte[] fileBytes)
    {
        System.IO.File.WriteAllBytes(filePath, fileBytes);
    }

    public static byte[] ReadFile(string filePath)
    {
        return System.IO.File.ReadAllBytes(filePath);
    }
}