using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace HyperDeobfuscator{
    public class RemoveProxies{
        public static void Run(ModuleDefMD Module){
            int count = 0;
            var methodsToDelete = new List<MethodDef>();
            foreach (var type in Module.Types){
                foreach (var method in type.Methods.Where(x => !x.IsNative && x.HasBody)){
                    var body = method.Body;
                    var instrs = body.Instructions;
                    for (int i = 0; i < instrs.Count; i++){
                        var instr = instrs[i];
                        if (instr.OpCode == OpCodes.Call){
                            try{
                                var call = (MethodDef)instr.Operand;
                                if (!isProxy(call))
                                    continue;
                                methodsToDelete.Add(call);
                                var content = (int)call.Body.Instructions[0].Operand;
                                instr.OpCode = call.Body.Instructions[0].OpCode;
                                instr.Operand = content;
                                count++;
                            }
                            catch (Exception e){
                                
                            }
                        }
                    }
                }

                
            }

            var moduleType = Module.GlobalType;
            foreach (var method in methodsToDelete){
                if (moduleType.Methods.Contains(method)){
                    moduleType.Methods.Remove(method);
                }
            }
            Console.WriteLine("Fixed " + count + " Proxies");
        }

        private static bool isProxy(MethodDef method){
            var instrs = method.Body.Instructions;
            if (instrs.Count == 2){
                if (isLdc(instrs[0]) && instrs[1].OpCode == OpCodes.Ret){
                    return true;
                }
            }
            return false;
        }
        public static bool isLdc(Instruction instr){
            var opcode = instr.OpCode;
            if (opcode == OpCodes.Ldc_I4)
                return true;
            if (opcode == OpCodes.Ldc_I4_S)
                return true;
            if (opcode == OpCodes.Ldc_I4_0)
                return true;
            if (opcode == OpCodes.Ldc_I4_1)
                return true;
            if (opcode == OpCodes.Ldc_I4_2)
                return true;
            if (opcode == OpCodes.Ldc_I4_3)
                return true;
            if (opcode == OpCodes.Ldc_I4_4)
                return true;
            if (opcode == OpCodes.Ldc_I4_5)
                return true;
            if (opcode == OpCodes.Ldc_I4_6)
                return true;
            if (opcode == OpCodes.Ldc_I4_7)
                return true;
            if (opcode == OpCodes.Ldc_I4_8)
                return true;
            return false;
        }
    }
}