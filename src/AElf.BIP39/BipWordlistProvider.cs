using System;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace AElf.BIP39;

public class BipWordlistProvider : IBipWordlistProvider, ITransientDependency
{
    public string[] LoadWordlist(BipWordlistLanguage language)
    {
        var file = language switch
        {
            BipWordlistLanguage.English => "english",
            BipWordlistLanguage.Japanese => "japanese",
            BipWordlistLanguage.Korean => "korean",
            BipWordlistLanguage.Spanish => "spanish",
            BipWordlistLanguage.ChineseSimplified => "chinese_simplified",
            BipWordlistLanguage.ChineseTraditional => "chinese_traditional",
            BipWordlistLanguage.French => "french",
            BipWordlistLanguage.Italian => "italian",
            BipWordlistLanguage.Czech => "czech",
            BipWordlistLanguage.Portuguese => "portuguese",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

        file = $"{file}.txt";
        return File.ReadAllLines(GetPath(file));
    }

    private string GetPath(string fileName)
    {
        return $"Wordlists/{fileName}";
    }
}