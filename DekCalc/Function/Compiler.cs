using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace DekCalc.Function
{
    public static class Compiler
    {
        public static List<MetadataReference> References { get; private set; } = new List<MetadataReference>();

        public static string ErrorMessage { get; private set; } = string.Empty;

        internal static Func<double, double, double, double, double, double, double>? CompileSimpleR2Function(string functionCode)
        {
            string codeText = Source.Replace("{0}", functionCode);

            Assembly? assembly = Compile(codeText);

            if(assembly == null)
                return null; // Signals error

            object? instance = assembly.CreateInstance("DekCalc.Function.Functions");

            var result = (Func<double, double, double, double, double, double, double>?)InvokeMethod(instance, "CreateSimpleFunction");

            return result;
        }

        private static Assembly? Compile(string codeText)
        {
            ErrorMessage = string.Empty;

#if NETFRAMEWORK
	        AddNetFrameworkDefaultReferences();
#else
            AddNetCoreDefaultReferences();
#endif
            AddAssembly(typeof(Math));
            AddAssembly(typeof(Complex));

            // Set up compilation Configuration
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(codeText.Trim());
            
            CSharpCompilation compilation = CSharpCompilation
                .Create(codeText)
                .WithAssemblyName("DekCalc")
                .WithOptions(new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release))
                .WithReferences(References)
                .AddSyntaxTrees(tree);

            Assembly assembly = null;
            bool isFileAssembly = false;

            using (Stream codeStream = new MemoryStream())
            {
                // Actually compile the code
                EmitResult compilationResult = compilation.Emit(codeStream);

                // Compilation Error handling
                if (!compilationResult.Success)
                {
                    var sb = new StringBuilder();
                    foreach (var diag in compilationResult.Diagnostics)
                    {
                        sb.AppendLine(diag.ToString());
                    }
                    ErrorMessage = sb.ToString();

                    return null;
                }

                // Load
                assembly = Assembly.Load(((MemoryStream)codeStream).ToArray());
                return assembly;
            }
        }

        public static void AddNetCoreDefaultReferences()
        {
            var rtPath = Path.GetDirectoryName(typeof(object).Assembly.Location) +
                         Path.DirectorySeparatorChar;

            AddAssemblies(
                rtPath + "System.Private.CoreLib.dll",
                rtPath + "System.Runtime.dll",
                rtPath + "System.Console.dll",
                rtPath + "netstandard.dll",

                rtPath + "System.Text.RegularExpressions.dll", // IMPORTANT!
                rtPath + "System.Linq.dll",
                rtPath + "System.Linq.Expressions.dll", // IMPORTANT!

                rtPath + "System.IO.dll",
                rtPath + "System.Net.Primitives.dll",
                rtPath + "System.Net.Http.dll",
                rtPath + "System.Private.Uri.dll",
                rtPath + "System.Reflection.dll",
                rtPath + "System.ComponentModel.Primitives.dll",
                rtPath + "System.Globalization.dll",
                rtPath + "System.Collections.Concurrent.dll",
                rtPath + "System.Collections.NonGeneric.dll",
                rtPath + "Microsoft.CSharp.dll"
            );

            // this library and CodeAnalysis libs
            AddAssembly(typeof(ReferenceList)); // Scripting Library
        }

        private static void AddAssemblies(params string[] assemblyPaths)
        {
            foreach(string path in assemblyPaths)
                AddAssembly(path);
        }

        public static bool AddAssembly(Type type)
        {
            try
            {
                //if (References.Any(r => r.FilePath == type.Assembly.Location))
                //    return true;

                var systemReference = MetadataReference.CreateFromFile(type.Assembly.Location);
                References.Add(systemReference);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool AddAssembly(string assemblyDll)
        {
            if (string.IsNullOrEmpty(assemblyDll)) return false;

            var file = Path.GetFullPath(assemblyDll);

            if (!File.Exists(file))
            {
                // check framework or dedicated runtime app folder
                var path = Path.GetDirectoryName(typeof(object).Assembly.Location);
                file = Path.Combine(path, assemblyDll);
                if (!File.Exists(file))
                    return false;
            }

            //if (References.Any(r => r.FilePath == file)) return true;

            try
            {
                var reference = MetadataReference.CreateFromFile(file);
                References.Add(reference);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static object? CreateInstance(string codeText, bool force = false)
        {

            return null;
        }

        public static Func<double, double, double, double, double, double, double>? GetFunction(this object me, string method, params object[] parameters)
        {
            return (Func<double, double, double, double, double, double, double>?)InvokeMethod(me, method, parameters);
        }

        public static object? InvokeMethod(object instance, string method, params object[] parameters)
        {
            ErrorMessage = string.Empty;

            if (instance == null)
            {
                ErrorMessage = "Can't invoke Script Method: Instance is null, and no previous call has cached the instance.";
                return null;
            }

            try
            {
                return instance.GetType().InvokeMember(method, BindingFlags.InvokeMethod, null, instance, parameters);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return null;
        }

        //public static object ExecuteMethod(string code, string methodName, params object[] parameters)
        //{
        //    ErrorMessage = "";

        //    object instance = ObjectInstance;

        //    if (instance == null)
        //    {
        //        // create self-contained class (see above)
        //        var sb = GenerateClass(code);

        //        // cache the instance if we have the same code   
        //        int hash = GenerateHashCode(code);
        //        if (!CachedAssemblies.ContainsKey(hash))
        //        {
        //            if (!CompileAssembly(sb.ToString()))
        //                return null;

        //            CachedAssemblies[hash] = Assembly;
        //        }
        //        else
        //        {
        //            Assembly = CachedAssemblies[hash];

        //            // Figure out the class name
        //            var type = Assembly.ExportedTypes.First();
        //            GeneratedClassName = type.Name;
        //            GeneratedNamespace = type.Namespace;
        //        }

        //        object tempInstance = CreateInstance();
        //        if (tempInstance == null)
        //            return null;
        //    }

        //    return InvokeMethod(ObjectInstance, methodName, parameters);
        //}
        public static Func<double, double[], double> CreateSimpleFunctionTEMPL()
        {
            return (x, _) => x;// {0};
        }

        public static string Source { get; set; } =
"""
using static System.Math;
using System.Numerics;

namespace DekCalc.Function
{
    public class Functions
    {
        //public delegate double DoubleFunction(double x, params string[] parameters);
        //public delegate Complex ComplexFunction(Complex z, params string[] parameters);

        public System.Func<double, double, double, double, double, double, double> CreateSimpleFunction()
        {
            return (x, A, B, C, D, E) => {0};
        }
    }
}
""";
        
    }
}
