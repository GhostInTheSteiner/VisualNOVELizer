using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptParser
{
    internal class ScriptLine : IEnumerable<ScriptLineWord>
    {
        public string Person { get; }
        public string PersonID { get; }
        public ScriptLineWord[] Content { get; }
        public string ContentString {
            get => 
                string.Join(" ", Content.Select(word => word.Content).ToArray()); }
        public bool IsQuoted { get; }
        public EllipsisStyle EllipsisStyle { get; }

        //public ScriptLine(string line, ParseMode parseMode) : this(line, "", parseMode)
        //{

        //}

        public ScriptLine(string line, ParseMode parseMode)
        {
            switch (parseMode)
            {
                case ParseMode.DoubleColon:
                    var splitted = line.Split(new[] { ": “" }, StringSplitOptions.None);
                    var lineRaw = string.Empty;

                    if (line.StartsWith("“") && line.EndsWith("”"))
                    {
                        //"<text>"
                        
                        lineRaw = line.Trim('“', '”');
                        
                        Person = "???";
                        PersonID = Guid.NewGuid().ToString();
                        Content = 
                            lineRaw
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();

                        IsQuoted = true;
                        EllipsisStyle = getEllipsisStyle(lineRaw);
                    }
                    else if (splitted.Length > 1)
                    {
                        //Akiho: "<text>"

                        lineRaw =
                            splitted
                                .Skip(1)
                                .First() //basically the entire rest
                                .Trim('“', '”');

                        Person = splitted.First();
                        PersonID = Person;
                        Content =
                            lineRaw
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();

                        IsQuoted = true;
                        EllipsisStyle = getEllipsisStyle(lineRaw);
                    }
                    else
                    {
                        //<text>

                        lineRaw = line;

                        Person = string.Empty;
                        PersonID = Person;
                        Content =
                            lineRaw
                                .Split(' ')
                                .Select(word =>
                                    new ScriptLineWord
                                    {
                                        Content = word
                                    })
                                .ToArray();

                        IsQuoted = false;
                        EllipsisStyle = getEllipsisStyle(lineRaw);
                    }

                    break;

                case ParseMode.SC3Output:
                    throw new NotImplementedException("SC3 won't ever be implemented!");
            }
        }

        private EllipsisStyle getEllipsisStyle(string lineRaw) =>

            lineRaw.StartsWith("...") && lineRaw.EndsWith("...") ?
                EllipsisStyle.OnBoth :

            lineRaw.StartsWith("...") ?
                EllipsisStyle.OnBegin :

            lineRaw.EndsWith("...") ?
                EllipsisStyle.OnEnd :

            /*else*/
                EllipsisStyle.None;

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
    
    internal enum EllipsisStyle { 
        None = 0,
        OnEnd = 1, 
        OnBegin = 2, 
        OnBoth = 4,
    }
}