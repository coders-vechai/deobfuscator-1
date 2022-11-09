using System;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace HyperDeobfuscator;

public class StringEncryption{
    public static void Run(ModuleDefMD Module){
        int count = 0;
        
        foreach (var type in Module.Types){
            foreach (var method in type.Methods.Where(x => !x.IsNative && x.HasBody)){
                var body = method.Body;
                var instrs = body.Instructions;
                for (int i = 0; i < instrs.Count; i++){
                    var instr = instrs[i];
                    if (instr.OpCode == OpCodes.Ldstr){
                        var call = instrs[i + 2];
                        var key = instrs[i + 1];
                        var content = (string)instr.Operand;
                        if (call.OpCode == OpCodes.Call && RemoveProxies.isLdc(key) && call.Operand.ToString().Contains("hyprObfuscatorCD")){
                            instr.Operand = Decrypt(content, (int)key.Operand);
                            call.OpCode = OpCodes.Nop;
                            key.OpCode = OpCodes.Nop;
                            count++;
                        }
                    }
                }
            }
        }
        Console.WriteLine("Fixed " + count + " Strings");
    }

    private static string Decrypt(string text, int key){
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char c in text)
        {
            stringBuilder.Append((char)((int)c ^ key));
        }
        return stringBuilder.ToString();
    }
}