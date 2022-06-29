using System;
using System.Collections.Generic;

namespace AElf.Cli;

public class AElfCliOptions
{
    public AElfCliOptions()
    {
        Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    }

    public Dictionary<string, Type> Commands { get; }
}