using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Desktop.Commands
{
    public class DebugCommand
    {
        public string Message { get; private set; }
        public DebugCommand(string msg)
        {
            Message = msg;
        }
    }
}
