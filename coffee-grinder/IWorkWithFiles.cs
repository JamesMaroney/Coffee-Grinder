using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coffee_grinder
{
  public interface IWorkWithFiles
  {
    string ReadFileToString(string path);
    void WriteStringToFile(string path, string content);
  }
}
