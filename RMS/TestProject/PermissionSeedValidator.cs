using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

internal static class PermissionSeedValidator
{
    public static bool Run()
    {
        Console.WriteLine("=== Permission Seed Validator ===");

        if (!TryResolveFiles(out string permissionKeysPath, out string permissionSeedPath, out string error))
        {
            Console.WriteLine($"ERROR: {error}");
            return false;
        }

        var codePermissions = ParsePermissionKeysFromCode(permissionKeysPath);
        var seededPermissions = ParseSeededPermissionsFromSql(permissionSeedPath);

        if (codePermissions.Count == 0)
        {
            Console.WriteLine("ERROR: No permissions found in clsPermissionKeys.cs.");
            return false;
        }

        if (seededPermissions.Count == 0)
        {
            Console.WriteLine("ERROR: No seeded permissions found in Permission_Seeding_Queries.sql.");
            return false;
        }

        var missingInSeed = codePermissions.Except(seededPermissions, StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        var extraInSeed = seededPermissions.Except(codePermissions, StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        Console.WriteLine($"Code permissions count : {codePermissions.Count}");
        Console.WriteLine($"Seed permissions count : {seededPermissions.Count}");

        if (missingInSeed.Count == 0 && extraInSeed.Count == 0)
        {
            Console.WriteLine("SUCCESS: clsPermissionKeys.cs matches Permission_Seeding_Queries.sql");
            return true;
        }

        if (missingInSeed.Count > 0)
        {
            Console.WriteLine("\nMissing in SQL seed (present in code):");
            foreach (var key in missingInSeed)
            {
                Console.WriteLine($"  - {key}");
            }
        }

        if (extraInSeed.Count > 0)
        {
            Console.WriteLine("\nExtra in SQL seed (not present in code):");
            foreach (var key in extraInSeed)
            {
                Console.WriteLine($"  - {key}");
            }
        }

        return false;
    }

    private static bool TryResolveFiles(out string permissionKeysPath, out string permissionSeedPath, out string error)
    {
        permissionKeysPath = string.Empty;
        permissionSeedPath = string.Empty;
        error = string.Empty;

        string? solutionRoot = FindDirectoryContaining(AppContext.BaseDirectory, "RMS.sln");
        if (string.IsNullOrWhiteSpace(solutionRoot))
        {
            error = "Could not locate RMS.sln from current execution directory.";
            return false;
        }

        string? repoRoot = Directory.GetParent(solutionRoot)?.FullName;
        if (string.IsNullOrWhiteSpace(repoRoot))
        {
            error = "Could not resolve repository root from solution root.";
            return false;
        }

        permissionKeysPath = Path.Combine(solutionRoot, "RMS_Business", "clsPermissionKeys.cs");
        permissionSeedPath = Path.Combine(repoRoot, "SQL Queries", "PermissionSeedingQueries", "Permission_Seeding_Queries.sql");

        if (!File.Exists(permissionKeysPath))
        {
            error = $"File not found: {permissionKeysPath}";
            return false;
        }

        if (!File.Exists(permissionSeedPath))
        {
            error = $"File not found: {permissionSeedPath}";
            return false;
        }

        return true;
    }

    private static string? FindDirectoryContaining(string startPath, string fileName)
    {
        var dir = new DirectoryInfo(startPath);

        while (dir != null)
        {
            string candidate = Path.Combine(dir.FullName, fileName);
            if (File.Exists(candidate))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static HashSet<string> ParsePermissionKeysFromCode(string filePath)
    {
        string content = File.ReadAllText(filePath);

        var matches = Regex.Matches(
            content,
            "public\\s+const\\s+string\\s+\\w+\\s*=\\s*\"(?<key>[^\"]+)\"\\s*;",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in matches)
        {
            string key = match.Groups["key"].Value.Trim();
            if (!string.IsNullOrWhiteSpace(key))
            {
                result.Add(key);
            }
        }

        return result;
    }

    private static HashSet<string> ParseSeededPermissionsFromSql(string filePath)
    {
        string content = File.ReadAllText(filePath);

        var blockMatch = Regex.Match(
            content,
            "INSERT\\s+INTO\\s+@PermissionSeed\\s*\\([^)]*\\)\\s*VALUES(?<values>[\\s\\S]*?);",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (!blockMatch.Success)
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        string valuesBlock = blockMatch.Groups["values"].Value;

        var keyMatches = Regex.Matches(
            valuesBlock,
            "\\(\\s*N'(?<key>[^']+)'\\s*,",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in keyMatches)
        {
            string key = match.Groups["key"].Value.Trim();
            if (!string.IsNullOrWhiteSpace(key))
            {
                result.Add(key);
            }
        }

        return result;
    }
}
