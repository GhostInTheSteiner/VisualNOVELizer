using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    class ApplicationConfiguration
    {
        public NameValueCollection Parser { get; }
        public NameValueCollection ParserChapters { get; }
        public NameValueCollection Writer { get; }

        public ApplicationConfiguration(
            NameValueCollection parser, 
            NameValueCollection parserChapters, 
            NameValueCollection writer
        )
        {
            this.Parser = parser;
            this.ParserChapters = parserChapters;
            this.Writer = writer;
        }
    }
}
