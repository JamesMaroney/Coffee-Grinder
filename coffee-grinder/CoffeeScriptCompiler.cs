using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jurassic;

namespace coffee_grinder
{
  public class CoffeeScriptCompiler
  {
    private IWorkWithFiles FileWorker;
    private ICompileStuff CsCompiler;

    public CoffeeScriptCompiler(IWorkWithFiles fileWorker, ICompileStuff csCompiler = null)
    {
      FileWorker = fileWorker;
      CsCompiler = csCompiler ?? new csCompiler();
    }


    public void Compile(string srcPath, string destPath)
    {
      try
      {
        var src = FileWorker.ReadFileToString(srcPath);
        var compiled = CsCompiler.Compile(src);
        FileWorker.WriteStringToFile(content: compiled, path: destPath); 
      }
      catch (IOException ex)
      {
        Console.WriteLine("IO Exception while handling event. Compilation may have failed.");
      }
    }

    public string GetJsPath(string coffeePath)
    {
      return new Regex(@"\.coffee$").Replace(coffeePath, ".js");
    }

    private class csCompiler : ICompileStuff
    {
      private static ScriptEngine jsEngine;
      private static ScriptEngine JsEngine
      {
        get { return jsEngine ?? (jsEngine = BuildJsEnvironment()); }
      }

      private static ScriptEngine BuildJsEnvironment()
      {
        var js = new Jurassic.ScriptEngine();
        js.Execute(Resources.coffeescript);
        return js;
      }

      public string Compile(string src)
      {
        JsEngine.SetGlobalValue("srcScript", src);
        var compiledScript = JsEngine.Evaluate<string>("CoffeeScript.compile(srcScript);");

        return compiledScript;
      }
    }
  }
}
