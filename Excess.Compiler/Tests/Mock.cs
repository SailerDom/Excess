﻿using Excess.Compiler;
using Excess.Compiler.Roslyn;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    using CSharp = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    public static class Mock
    {
        public static SyntaxTree CompileWithMapping(string code)
        {
            var compiler = new RoslynCompiler();
            var document = new RoslynDocument(compiler.Scope, code);
            var mapper = document.Mapper = new MappingService();

            compiler.apply(document);
            document.applyChanges();

            var result = mapper.RenderMapping(document.SyntaxRoot, string.Empty);
            return CSharp.ParseCompilationUnit(result).SyntaxTree;
        }
    }
}
