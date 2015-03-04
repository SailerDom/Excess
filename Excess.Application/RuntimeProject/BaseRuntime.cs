﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Compilation = Excess.Compiler.Roslyn.Compilation;
using Excess.Compiler;
using Excess.Compiler.XS;

namespace Excess.RuntimeProject
{
    internal abstract class BaseRuntime : IRuntimeProject
    {
        public BaseRuntime()
        {
            _compilation.addSyntaxTree(consoleTree);
            _compilation.addSyntaxTree(randomTree);
        }

        public bool busy()
        {
            return _busy;
        }

        protected Compilation _compilation = new Compilation();

        public IEnumerable<Error> compile()
        {
            if (_busy)
                throw new InvalidOperationException();

            _busy = true;
            IEnumerable<Error> errors = null;
            try
            {
                errors = doCompile();
            }
            catch(Exception e)
            {
                errors = new[] { new Error {
                    File = "compiler",
                    Line = -1,
                    Message = e.Message,
                }};
            }

            _busy = false;
            return errors;
        }

        public IEnumerable<Error> run(out dynamic client)
        {
            client = null;
            if (_busy)
                throw new InvalidOperationException();

            _busy = true;
            IEnumerable<Error> errors = null;
            try
            {
                errors = doCompile();
                if (errors == null)
                {
                    Assembly assembly = _compilation.build();
                    if (assembly != null)
                    {
                        setupConsole(assembly);
                        doRun(assembly, out client);
                    }
                    else
                        errors = gatherErrors(_compilation.errors());
                }
            }
            catch (Exception e)
            {
                errors = new[] { new Error {
                    File = "compiler",
                    Line = -1,
                    Message = e.Message,
                }};
            }

            _busy = false;
            return errors;
        }

        private IEnumerable<Error> gatherErrors(IEnumerable<Diagnostic> diagnostics)
        {
            if (diagnostics == null)
                return null;

            return diagnostics
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .Select(diagnostic =>
                {
                    var mapped = _compilation.OriginalPosition(diagnostic.Location);
                    return new Error
                    {
                        File = mapped.Path,
                        Line = mapped.StartLinePosition.Line,
                        Character = mapped.StartLinePosition.Character,
                        Message = diagnostic.GetMessage()
                    };
                });
        }

        private class ExtensionFile
        {
            public int Id { get; set; }
            public string Contents { get; set; }
        }

        private Dictionary<string, ExtensionFile> _extensionFiles = new Dictionary<string, ExtensionFile>();
        public void add(string file, int fileId, string contents)
        {
            if (_files.ContainsKey(file))
                throw new InvalidOperationException();

            var ext = Path.GetExtension(file);
            if (string.IsNullOrEmpty(ext))
                _compilation.addDocument(file, contents, getInjector(file));
            else
                _extensionFiles[file] = new ExtensionFile { Id = fileId, Contents = contents };

            _files[file] = fileId;
            _dirty = true;
        }

        protected virtual ICompilerInjector<SyntaxToken, SyntaxNode, SemanticModel> getInjector(string file)
        {
            return XSModule.Create();
        }

        public void modify(string file, string contents)
        {
            if (!_files.ContainsKey(file))
                throw new InvalidOperationException();

            if (_compilation.hasDocument(file))
                _compilation.updateDocument(file, contents);
            else
                _extensionFiles[file].Contents = contents; //td: move to compilation

            _dirty = true;
        }

        public IEnumerable<Notification> notifications()
        {
            List<Notification> result = new List<Notification>();
            lock (_notificationLock)
            {
                int toRemove = 0;
                foreach (var not in _notifications)
                {
                    result.Add(not);
                    toRemove++;

                    if (not.Kind == NotificationKind.Finished)
                        break;
                }

                if (toRemove > 0)
                    _notifications.RemoveRange(0, toRemove);
            }

            return result;
        }

        public abstract string defaultFile();

        public string fileContents(string file)
        {
            var result = null as string;
            if (_files.ContainsKey(file))
            {
                result = _compilation.documentText(file);
                if (result == null)
                {
                    ExtensionFile ext;
                    if (_extensionFiles.TryGetValue(file, out ext))
                        return ext.Contents;
                }
            }

            return result;
        }

        public int fileId(string file)
        {
            int result;
            if (_files.TryGetValue(file, out result))
                return result;

            return -1;
        }

        public virtual IEnumerable<TreeNodeAction> fileActions(string file)
        {
            return new TreeNodeAction[] { };
        }

        protected bool _busy  = false;
        protected bool _dirty = false;

        protected static Dictionary<string, IFileExtension> _fileExtensions = new Dictionary<string, IFileExtension>();

        protected virtual IEnumerable<Error> doCompile()
        {
            if (!_dirty)
            {
                notify(NotificationKind.System, "Compilation up to date");
                return null;
            }

            //process extensioned files
            foreach (var file in _files)
            {
                var filename = file.Key;
                var ext = Path.GetExtension(filename);
                if (string.IsNullOrEmpty(ext))
                    continue;

                var info = _extensionFiles[filename];
                var handler = null as IFileExtension;
                if (_fileExtensions.TryGetValue(ext, out handler))
                {
                    handler.process(filename, info.Id, info.Contents, _compilation);
                }
            }


            var result = _compilation.compile();
            if (_dirty)
                _dirty = false;

            if (result)
                return null;

            return gatherErrors(_compilation.errors());
        }

        public void setFilePath(dynamic path)
        {
            _compilation.setPath(path);
        }

        protected abstract void doRun(Assembly asm, out dynamic clientData);
        
        protected Dictionary<string, int> _files            = new Dictionary<string, int>();
        private List<Notification>        _notifications    = new List<Notification>();
        private object                    _notificationLock = new object();

        protected void notify(NotificationKind kind, string message)
        {
            lock(_notificationLock)
            {
                _notifications.Add(new Notification { Kind = kind, Message = message });
            }
        }

        //Console
        static protected SyntaxTree consoleTree = SyntaxFactory.ParseSyntaxTree(
            @"using System;
            public class console
            {
                static private Action<string> _notify;
                static public void setup(Action<string> notify)
                {
                    _notify = notify;
                }

                static public void write(string message)
                {
                    _notify(message);
                }
            }");

        static protected SyntaxTree randomTree = SyntaxFactory.ParseSyntaxTree(
            @"using System;
            public class random
            {
                static private Random _random = new Random();

                static public int Int()
                {
                    return _random.Next();
                }

                static public int Int(int range)
                {
                    return _random.Next(range);
                }

                static public double Double()
                {
                    return _random.NextDouble();
                }
            }");

        private void setupConsole(Assembly assembly)
        {
            Type console = assembly.GetType("console");
            var  method = console.GetMethod("setup");
            method.Invoke(null, new [] { (Action<string>)consoleNotify });
        }

        private void consoleNotify(string message)
        {
            notify(NotificationKind.Application, message);
        }
    }
}
