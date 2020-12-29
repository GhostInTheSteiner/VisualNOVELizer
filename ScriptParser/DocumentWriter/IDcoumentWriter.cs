using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    internal interface IDocumentWriter
    {
        void Export(ScriptBook book, string filePath);
    }
}
