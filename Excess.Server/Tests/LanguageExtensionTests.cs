﻿using Excess.Compiler.Core;
using Excess.Compiler.Roslyn;
using LanguageExtension;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class LanguageExtensionTests
    {
        [TestMethod]
        public void Usage()
        {
            //setup
            const string Code = @"
            using XS.Extensions.Server;

            namespace SomeNS            
            {
                server SomeServer()
                {
                    Url = ""http://*.1080"";
                
                    Node someNode = new NetMQ.RequestResponse
                    {
                        Url = ""http://*.2080"",
                        Threads = 25,
                        Hosts = new []
                        {
                            SomeService
                        }
                    };
                }
            }";

            string output;
            var tree = Mock.Compile(Code, out output);

            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void WithNodes()
        {
            //setup
            const string Source = @"
            concurrent class HelloService
            {
                public string Hello(string what)
                {
                    return ""Hello "" + what;
                }
            }

            concurrent class GoodbyeService
            {
                public string Goodbye(string what)
                {
                    return ""Goodbye "" + what;
                }
            }

            namespace Servers
            {
                server Default()
                {
                    Url = ""http://localhost:1080"";

                    Node node1 = new NetMQ.RequestResponse
                    {
                        Url = ""http://localhost:1081"",
                        Hosts = new []
                        {
                            HelloService
                        }
                    };

                    Node node2 = new NetMQ.RequestResponse
                    {
                        Url = ""http://localhost:1082"",
                        Hosts = new []
                        {
                            GoodbyeService
                        }
                    };
                }
            }";

            var errorList = new List<string>();
            var compilation = Mock.Build(Source, errors: errorList);
            
            Assert.IsFalse(errorList.Any());

            var serverConfig = compilation.Scope.get<IServerConfiguration>();
            Assert.IsNotNull(serverConfig);

            var clientCode = serverConfig.GetClientInterface();

            Assert.IsTrue(
                clientCode.Contains("function HelloService (__init)")
                && clientCode.Contains("function GoodbyeService (__init)")
                && clientCode.Contains("Hello = function (who)")
                && clientCode.Contains("Goodbye = function (what)"));
        }

        [TestMethod]
        public void WithComplexTypes()
        {
            //setup
            const string Source = @"
            struct HelloModel
            {
                public string Greeting;                
                public int Times;
                public GoodbyeService Goodbye;
            }

            concurrent class HelloService
            {
                int _times = 0;
                public HelloModel Hello(string who)
                {
                    return new HelloModel
                    {
                        Greeting = ""greetings, "" + who,
                        Times = _times++,
                        Goodbye = spawn<GoodbyeService>()
                    };
                }
            }

            concurrent class GoodbyeService
            {
                public string Name = ""GoodbyeService""; 

                public string Goodbye(string what)
                {
                    return ""Goodbye "" + what;
                }
            }";

            var storage = new MockStorage();
            var errors = new List<string>();
            var analysis = new CompilationAnalysisBase<SyntaxToken, SyntaxNode, SemanticModel>();
            var compilation = Mock.Build(Source, storage, errors, analysis);

            Assert.IsFalse(errors.Any());

            var serverConfig = compilation.Scope.GetServerConfiguration();
            Assert.IsNotNull(serverConfig);

            compilation.PerformAnalysis();

            var clientCode = serverConfig.GetClientInterface();

            Assert.IsTrue(
                clientCode.Contains("function HelloService (__init)")
                && clientCode.Contains("function GoodbyeService (__init)")
                && clientCode.Contains("Hello = function (who)")
                && clientCode.Contains("Goodbye = function (what)"));
        }

        [TestMethod]
        public void ServiceUsage()
        {
            //setup
            const string Code = @"
            service SomeService
            {
                public void someMethod()
                {
                }
            }";

            string output;
            var tree = Mock.Compile(Code, out output);

            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void DebugPrint()
        {
            string text;
            Mock.Compile(@"
            public service TestService
            {
                public string Hello(string what)
                {
                    return ""Hello "" + what;
                }
            }", out text);

            Assert.IsNotNull(text);
        }
    }
}
