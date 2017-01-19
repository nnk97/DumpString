using System;
using System.Text;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DumpStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            // Last args part is our path, maybe I'll add some - options in future...
            var ExePath = args[args.Length - 1];
            if (!File.Exists(ExePath))
            {
                Console.WriteLine($"File doesn't exist!\nPath => {ExePath}");
                return;
            }

            try
            {
                string OutputPath = ExePath.Substring(0, ExePath.Length - 4) + ".txt";

                if (File.Exists(OutputPath))
                    File.Delete(OutputPath);

                var UTFEncoder = new UTF8Encoding(true);

                using (FileStream fs = File.Create(OutputPath))
                {
                    ModuleDefMD Module = ModuleDefMD.Load(ExePath);

                    foreach (TypeDef Type in Module.GetTypes())
                    {
                        foreach (MethodDef Method in Type.Methods)
                        {
                            if (!Method.HasBody)
                                continue;

                            foreach (Instruction Instr in Method.Body.Instructions)
                            {
                                if (Instr.OpCode == OpCodes.Ldstr)
                                {
                                    // (string)Instr.Operand <= this holds string "value"
                                    var String = $"[{Type.Name}.{Method.Name}] {(string)Instr.Operand}{Environment.NewLine}";
                                    Console.WriteLine(String);

                                    byte[] Bytes = UTFEncoder.GetBytes(String);
                                    fs.Write(Bytes, 0, Bytes.Length);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
