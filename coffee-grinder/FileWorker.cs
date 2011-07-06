using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace coffee_grinder
{
  class FileWorker : IWorkWithFiles
  {
    public string ReadFileToString(string path)
    {
      return File.ReadAllText(path);
    }

    public void WriteStringToFile(string path, string content)
    {
      File.WriteAllText(path, content);
    }
  }
}
