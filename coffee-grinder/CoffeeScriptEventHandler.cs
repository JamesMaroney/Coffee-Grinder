using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace coffee_grinder
{
  public class CoffeeScriptEventHandler
  {
    private CoffeeScriptCompiler Compiler;

    public CoffeeScriptEventHandler(CoffeeScriptCompiler compiler)
    {
      Compiler = compiler;
    }

    public void onCreated(object sender, FileSystemEventArgs e)
    {
      Console.WriteLine("Processing Created Event for: {0}", e.FullPath);
      Compiler.Compile(e.FullPath, Compiler.GetJsPath(e.FullPath));
    }
    public void onChanged(object sender, FileSystemEventArgs e)
    {
      Console.WriteLine("Processing Changed Event for: {0}", e.FullPath);
      Compiler.Compile(e.FullPath, Compiler.GetJsPath(e.FullPath));
    }
    public void onDeleted(object sender, FileSystemEventArgs e)
    {
      Console.WriteLine("Processing Deleted Event for: {0}", e.FullPath);
      File.Delete(Compiler.GetJsPath(e.FullPath));
    }
    public void onRenamed(object sender, RenamedEventArgs e)
    {
      Console.WriteLine("Processing Renamed Event for: {0} -> {1}", e.OldFullPath, e.FullPath);
      File.Delete(Compiler.GetJsPath(e.OldFullPath));
      Compiler.Compile(e.FullPath, Compiler.GetJsPath(e.FullPath));
    }
  }
}
