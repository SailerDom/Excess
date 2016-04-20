﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Excess.Compiler.Roslyn;

namespace xsc
{
    using ExcessCompilation = Excess.Compiler.Roslyn.Compilation;

    public class Runner
    {
        public IEnumerable<string> Files { get; set; }
        public string SolutionFile { get; set; }
        public IDictionary<string, Action<RoslynCompiler>> Extensions { get; set; }
        public IDictionary<string, string> Flavors { get; set; }
        public string OutputPath { get; set; }
        public bool Transpile { get; set; }

        public Runner()
        {
            Flavors = new Dictionary<string, string>();
        }

        string _directory;
        public IEnumerable<string> directoryFiles(string directory = null)
        {
            if (directory == null)
                directory = Environment.CurrentDirectory;

            _directory = directory;
            var validExtensions = new[] { ".cs", ".xs" };
            return Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                .Where(file => validExtensions.Contains(Path.GetExtension(file))
                            && !file.Contains("Generated"));
        }

        public IDictionary<string, Action<RoslynCompiler>> directoryExtensions(string directory)
        {
            var items = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                .Where(file => Path.GetExtension(file) == ".dll"
                            && Path.GetFileName(file).StartsWith("Excess.Extensions"))
                .Select(dll => loadExtension(dll));

            var result = new Dictionary<string, Action<RoslynCompiler>>();
            foreach (var item in items)
            {
                if (item.Equals(default(KeyValuePair<string, Action<RoslynCompiler>>)))
                    continue;

                result[item.Key] = item.Value;
            }

            return result;
        }

        private KeyValuePair<string, Action<RoslynCompiler>> loadExtension(string dll)
        {
            var assembly = Assembly.LoadFrom(dll);

            var id = Path
                .GetFileNameWithoutExtension(dll)
                .Substring("Excess.Extensions.".Length)
                .ToLower();

            var flavorType = assembly
                .GetTypes()
                .Where(type => type.Name == "Flavors")
                .SingleOrDefault();

            var method = null as MethodInfo;
            if (flavorType != null)
            {
                string flavor;
                if (Flavors.TryGetValue(id, out flavor))
                {
                    method = getStaticMethod(flavorType, flavor);
                    if (method == null)
                        throw new InvalidOperationException($"invalid flavor: {flavor} for {id}");

                    Flavors.Remove(id);
                }
                else
                    throw new InvalidOperationException($"invalid flavor: {flavor} for {id}");
            }

            if (method == null)
            {
                var extensionType = assembly
                    .GetTypes()
                    .Where(type => type.Name == "Extension")
                    .SingleOrDefault();

                if (extensionType != null)
                    method = getStaticMethod(extensionType, "Apply");
            }

            if (method == null)
                return default(KeyValuePair<string, Action<RoslynCompiler>>);

            return new KeyValuePair<string, Action<RoslynCompiler>>(id,
                compiler => method.Invoke(null, new object[] { compiler }));
        }

        private MethodInfo getStaticMethod(Type type, string id)
        {
            return type
                .GetMethods()
                .Where(m =>
                {
                    if (m.Name != id || !m.IsStatic || !m.IsPublic)
                        return false;

                    var parameters = m.GetParameters();
                    return parameters.Length == 1
                        && parameters[0].ParameterType.Name == "RoslynCompiler";
                }).SingleOrDefault();
        }

        public void buildSolution()
        {
            if (SolutionFile == null || !File.Exists(SolutionFile))
                throw new InvalidProgramException($"invalid solution file: {SolutionFile ?? "null"}");

            throw new NotImplementedException();
        }

        public void buildFiles()
        {
            if (Files == null)
                throw new InvalidProgramException("must specify which files to compile");

            var actualFiles = Files.ToArray();
            if (actualFiles.Length == 0)
                throw new InvalidProgramException($"must specify which files to compile");

            var asExe = actualFiles
                .Where(path => Path
                    .GetFileName(path)
                    .Equals("Program.cs"))
                .Any();

            if (Extensions == null)
            {
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                Extensions = directoryExtensions(exePath);
            }

            var compilation = new ExcessCompilation(
                extensions: Extensions,
                executable: asExe);

            foreach (var file in actualFiles)
            {
                if (file.EndsWith(".xs.cs"))
                    continue;

                var ext = Path.GetExtension(file);
                switch (ext)
                {
                    case ".cs": compilation.addCSharpFile(file); break;
                    case ".xs": compilation.addDocument(file); break;

                    default: throw new InvalidOperationException($"invalid extension: {ext}");
                }
            }

            var errors = null as IEnumerable<Diagnostic>;
            var result = compilation.build(out errors);
            if (result != null)
            {
                if (Transpile)
                {
                    foreach (var document in compilation.Documents())
                    {
                        var filename = compilation.DocumentFileName(document) + ".cs";
                        Console.WriteLine($"Generated: {filename}");

                        var text = document.SyntaxRoot.NormalizeWhitespace().ToFullString();
                        File.WriteAllText(filename, text);
                    }
                }
                else
                {
                    var outputPath = OutputPath;
                    if (outputPath == null)
                        outputPath = Path.Combine(
                            _directory,
                            Path.GetFileName(_directory));

                    if (Path.GetExtension(outputPath) == string.Empty)
                        outputPath = outputPath + (asExe ? ".exe" : ".dll");

                    Debug.Assert(outputPath != null);

                    outputPath = Path.GetFullPath(outputPath);
                    File.WriteAllBytes(outputPath, result.GetBuffer());
                    Console.WriteLine($"Successfully built: {outputPath}");
                }
            }
            else
            {
                foreach (var error in errors)
                    Console.Error.WriteLine(error.ToString());
            }
        }
    }
}
