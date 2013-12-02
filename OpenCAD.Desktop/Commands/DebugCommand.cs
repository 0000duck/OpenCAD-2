using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Desktop.Commands
{
    internal class DebugCommand
    {
        public string Message { get; private set; }
        public DebugCommand(string msg)
        {
            Message = msg;
        }
    }
}
