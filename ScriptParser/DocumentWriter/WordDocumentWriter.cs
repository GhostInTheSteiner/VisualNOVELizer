using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    internal class WordDocumentWriter : IDocumentWriter
    {
        public void Export(ScriptBook book, string filePath)
        {
            throw new NotImplementedException("Word export won't ever be implemented!");
        }
    }
}
