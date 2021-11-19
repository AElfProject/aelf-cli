# AElf.Cli

## 通过本地代码安装aelf命令

1. cd到src/AElf.Cli目录下，执行`dotnet pack`

2. 当前目录生成`nupkg`文件夹，且文件夹中包含文件`AElf.Cli.*.nupkg`，执行：
```shell
dotnet tool install --global --add-source ./nupkg aelf.cli
```

3. 以上步骤成功以后，即可使用`aelf`命令，如启动一个单节点：

```shell
aelf start
```

4. 修改代码后，需要重新执行pack，然后使用`dotnet tool update`升级aelf命令：

```shell
dotnet pack
dotnet tool update --global --add-source ./nupkg aelf.cli
```

## 添加新的命令

1. 在Commands目录下添加新的文件`*Command.cs`，让它继承自`IConsoleCommand`接口；

2. 分别实现以下三个方法：
- ExecuteAsync：执行该命令的逻辑
- GetUsageInfo：该命令的帮助文档，如使用方法等
- GetShortDescription：该命令的简单介绍

3. 在`AElfCliModule.cs`文件中注册该命令，格式参考HelpCommand、StartCommand等。

4. 使用`dotnet pack`和`dotnet tool update ...`命令重新安装到本地即可测试新的命令。