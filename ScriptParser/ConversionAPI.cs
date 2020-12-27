using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptParser
{
    internal class ConversionAPI
    {
        private ScriptParser parser;
        private DocumentWriter writer;

        public ConversionAPI(ScriptParser parser, DocumentWriter writer)
        {
            this.parser = parser;
            this.writer = writer;
        }

        internal ScriptText Parse(string directory)
        {
            return parser.Parse(directory);
        }

        internal void Export(ScriptText text, string v)
        {
            throw new NotImplementedException();
        }
    }
}