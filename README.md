# AElf.Cli

BRANCH | AZURE PIPELINES                                                                                                                                                                                                     | TESTS                                                                                                                                                                                  | CODE COVERAGE
-------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------
MASTER   | [![Build Status](https://dev.azure.com/AElfProject/aelf-cli/_apis/build/status/AElfProject.aelf-cli?branchName=master)](https://dev.azure.com/AElfProject/aelf-cli/_build/latest?definitionId=24&branchName=master) | [![Test Status](https://img.shields.io/azure-devops/tests/AElfProject/aelf-cli/24/master)](https://dev.azure.com/AElfProject/aelf-cli/_build/latest?definitionId=24&branchName=master) | [![codecov](https://codecov.io/gh/AElfProject/aelf-cli/branch/master/graph/badge.svg?token=TqF7wG35aW)](https://codecov.io/gh/AElfProject/aelf-cli)
DEV    | [![Build Status](https://dev.azure.com/AElfProject/aelf-cli/_apis/build/status/AElfProject.aelf-cli?branchName=dev)](https://dev.azure.com/AElfProject/aelf-cli/_build/latest?definitionId=24&branchName=dev)       | [![Test Status](https://img.shields.io/azure-devops/tests/AElfProject/aelf-cli/24/dev)](https://dev.azure.com/AElfProject/aelf-cli/_build/latest?definitionId=24&branchName=dev)       | [![codecov](https://codecov.io/gh/AElfProject/aelf-cli/branch/dev/graph/badge.svg?token=TqF7wG35aW)](https://codecov.io/gh/AElfProject/aelf-cli)

This tool is still working in process, please add this myget source if you occur problems with finding packages:
```
https://www.myget.org/F/aelf-project-dev/api/v3/index.json
```

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