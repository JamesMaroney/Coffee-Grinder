using coffee_grinder;
using Moq;
using NSpec;

namespace Tests
{
  class describe_CoffeeScriptCompiler : nspec
  {
    private CoffeeScriptCompiler Compiler;
    private Mock<IWorkWithFiles> MockFileWorker;
    private Mock<ICompileStuff> MockCompiler;

    void describe_Compile()
    {
      before = () =>
      {
        MockFileWorker = new Mock<IWorkWithFiles>();
        MockCompiler = new Mock<ICompileStuff>();
        Compiler = new CoffeeScriptCompiler(MockFileWorker.Object, MockCompiler.Object);
      };

      context["When compiling a coffeescript file"] = () =>
      {
        string compiledPath = "compiledPath";
        string srcPath = "srcPath";
        string src = "src";
        string compiled = "compiled";

        before = () =>
                   {
                     MockFileWorker.Setup(fw => fw.ReadFileToString(srcPath)).Returns(src);
                     MockFileWorker.Setup(fw => fw.WriteStringToFile(compiled,compiledPath));
                     MockCompiler.Setup(fw => fw.Compile(src)).Returns(compiled);

                     Compiler.Compile(srcPath, compiledPath);
                   };

        it["should read in the source file"] = 
          () => MockFileWorker.Verify(fw => fw.ReadFileToString(srcPath));
        it["should write to the destination file"] = 
          () => MockFileWorker.Verify(fw => fw.WriteStringToFile(compiledPath, It.IsAny<string>()));
        it["should write the compiled javascript to the destination file"] =
          () => MockFileWorker.Verify(fw => fw.WriteStringToFile(It.IsAny<string>(), compiled));
      };
    }

    void describe_GetJsPath()
    {
      before = () =>
                 {
                   Compiler = new CoffeeScriptCompiler(new Mock<IWorkWithFiles>().Object,
                                                       new Mock<ICompileStuff>().Object);
                 };

      context["when the path ends in .coffee"] = () =>
      {
        string jsPath = "";
        before = () => jsPath = Compiler.GetJsPath(@"C:\path\file.coffee");

        it["should replace the extension"] = () => jsPath.should_be(@"C:\path\file.js");

        context["and coffee appears elsewhere in the filename"] = () =>
        {
          before = () => jsPath = Compiler.GetJsPath(@"C:\path\project.coffee\file.coffee");

          it["should only replace the file extension"] = () => jsPath.should_be(@"C:\path\project.coffee\file.js");
        };
      };
    }
  }
}
