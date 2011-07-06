
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace coffee_grinder
{
  class Program
  {

    static void Main(string[] args)
    {
      RegisterEmbeddedResourceAssemblyLoading();
      var coffeescriptEventHandler = new CoffeeScriptEventHandler(new CoffeeScriptCompiler(new FileWorker()));
      WatchDir(Environment.CurrentDirectory, coffeescriptEventHandler, "*.coffee");

      Console.WriteLine("Watching current directory for coffeescript file events.");
      Thread.Sleep(-1);
    }

    private static void RegisterEmbeddedResourceAssemblyLoading()
    {
      AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
      {
        String resourceName = "coffee_grinder.lib." +
           new AssemblyName(args.Name).Name + ".dll";

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        {
          Byte[] assemblyData = new Byte[stream.Length];
          stream.Read(assemblyData, 0, assemblyData.Length);
          return Assembly.Load(assemblyData);
        }
      };
    }

    static void WatchDir(string path,  CoffeeScriptEventHandler eventHandler, string filter = null)
    {
      var watcher = new FileSystemWatcher(path);
      watcher.IncludeSubdirectories = true;

      if (null != filter) watcher.Filter = filter;
      watcher.Changed += eventHandler.onChanged;
      watcher.Created += eventHandler.onCreated;
      watcher.Deleted += eventHandler.onDeleted;
      watcher.Renamed += eventHandler.onRenamed;

      watcher.EnableRaisingEvents = true;
    }
  }
}
