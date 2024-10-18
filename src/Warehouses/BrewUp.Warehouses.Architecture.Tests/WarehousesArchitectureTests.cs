using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NetArchTest.Rules;

namespace BrewUp.Warehouses.Architecture.Tests;

[ExcludeFromCodeCoverage]
public class WarehousesArchitectureTests
{
    [Fact]
    public void WarehousesProjects_Should_Having_Namespace_StartingWith_BrewUp_Sales()
    {
        var sourceModulePath = Path.Combine(VisualStudioProvider.TryGetSolutionDirectoryInfo().FullName, "Warehouses");
        var subFolders = Directory.GetDirectories(sourceModulePath);

        var netVersion = Environment.Version;

        var moduleAssemblies = (from folder in subFolders
                                   let binFolder = Path.Join(folder, "bin", "Debug", $"net{netVersion.Major}.{netVersion.Minor}")
                                   let files = Directory.GetFiles(binFolder)
                                   let folderArray = folder.Split(Path.DirectorySeparatorChar)
                                   select files.FirstOrDefault(f => f.EndsWith($"{folderArray[folderArray!.Length - 1]}.dll"))
            into assemblyFilename
                                   where !assemblyFilename!.Contains("Test")
                                   select Assembly.LoadFile(assemblyFilename!)).ToList();

        var moduleTypes = Types.InAssemblies(moduleAssemblies);
        var moduleResult = moduleTypes
            .Should()
            .ResideInNamespaceStartingWith("BrewUp.Warehouses")
            .GetResult();

        Assert.True(moduleResult.IsSuccessful);
    }

    [Fact]
    public void Should_WarehousesArchitecture_BeCompliant()
    {
        var types = Types.InAssembly(typeof(Facade.WarehousesFacade).Assembly);

        var forbiddenAssemblies = new List<string>
        {
            "BrewUp.Sales.Facade",
            "BrewUp.Sales.Domain",
            "BrewUp.Sales.Infrastructures",
            "BrewUp.Sales.ReadModel",
            "BrewUp.Sales.SharedKernel"
        };

        var result = types
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenAssemblies.ToArray())
            .GetResult()
            .IsSuccessful;

        Assert.True(result);
    }

    private static class VisualStudioProvider
    {
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string? currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}