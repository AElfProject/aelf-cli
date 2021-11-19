# AElf.Cli

## How to install aelf command in local

1. `cd` to the dir `src/AElf.Cli`, run `dotnet pack`

2. Run this sh in same dir
```shell
dotnet tool install --global --add-source ./nupkg aelf.cli
```

3. Now you can use `aelf` cli

```shell
aelf start
```

4. `dotnet pack` need to be run again if you modify the code, then use `dotnet tool update` to update `aelf` cli

```shell
dotnet pack
dotnet tool update --global --add-source ./nupkg aelf.cli
```

## How to add new command

1. Add `*Command.cs` file to `Commands` dir and make this class inheri from `IConsoleCommand`

2. Impl following 3 methods
- ExecuteAsync：The logic of executing this command
- GetUsageInfo：Get more information about this command
- GetShortDescription：Short decription of this command

3. Then register this command to `AElfCliModule.cs`, the format you can ref to HelpCommand and StartCommand

4. `dotnet pack` & `dotnet tool update ...` as stated before