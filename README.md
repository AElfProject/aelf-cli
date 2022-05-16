# AElf.Cli

## How to install aelf command in local

1. `cd` to the dir `src/AElf.Cli`, run `dotnet pack`

2. Run following command in same dir
```shell
dotnet tool install --global --add-source ./nupkg aelf.cli
```

3. Now you can use `aelf` cli

```shell
aelf start
```

Watch, you need to add your aelf keystore file (a json file) to `aelf/keys` dir, and config the address and password to `src/AElf.Cli/appsettings.json` file.

4. `dotnet pack` need to be run again if you modify the code, then use `dotnet tool update` to update `aelf` cli

```shell
dotnet pack
dotnet tool update --global --add-source ./nupkg aelf.cli
```

## How to add new command

1. Add `*Command.cs` file to `Commands` dir and make this class inherit from `IAElfCommand`

2. Impl following 3 methods
- ExecuteAsync：The logic of executing this command
- GetUsageInfo：Get more information about this command
- GetShortDescription：Short decription of this command

3. Then register this command to `AElfCliModule.cs`, the format you can ref to HelpCommand and StartCommand

4. `dotnet pack` & `dotnet tool update --global --add-source ./nupkg aelf.cli` as stated before