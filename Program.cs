using System;
using System.Diagnostics;
using System.Timers;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace HyperDeobfuscator{
    internal class Program{
        public static void Main(string[] args){
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var module = ModuleDefMD.Load(args[0]);
            RemoveProxies.Run(module);
            StringEncryption.Run(module);
            var options = new ModuleWriterOptions(module);
            options.Logger = DummyLogger.NoThrowInstance;
            module.Write(args[0].Replace(".exe","-Deobfuscated.exe"), options);
            stopwatch.Stop();
            Console.WriteLine("Process finished in " + stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}