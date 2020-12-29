using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptParser
{
    internal class ScriptLine : IEnumerable<ScriptLineWord>
    {
        public string Person { get; }
        public ScriptLineWord[] Content { get; }
        public string ContentString {
            get => 
                string.Join(" ", Content.Select(word => word.Content).ToArray()); }

        public ScriptLine(string line, ParseMode parseMode)
        {
            switch (parseMode)
            {
                case ParseMode.DoubleColon:
                    var splitted = line.Split(new[] { ": “" }, StringSplitOptions.None);

                    if (line.StartsWith("“") && line.EndsWith("”"))
                    {
                        Person = "???";
                        Content =
                            line
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();
                    }
                    else if (splitted.Length > 1)
                    {
                        Person = splitted.First();
                        Content =
                            splitted
                                .Skip(1)
                                .First() //basically the entire rest
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();
                    }
                    else
                    {
                        Person = string.Empty;
                        Content =
                            line
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();
                    }

                    break;

                case ParseMode.SC3Output:
                    throw new NotImplementedException("SC3 won't ever be implemented!");
            }
        }

        public IEnumerator<ScriptLineWord> GetEnumerator()
        {
            return ((IEnumerable<ScriptLineWord>)Content).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Content.GetEnumerator();
        }
    }

    internal enum ParseMode { DoubleColon, SC3Output }
}