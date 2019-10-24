using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StateMachine.MdlReader
{
    enum State
    {
        START_UP,
        END_OFF,
        EMPTY,
        REMAINING,
        TEXT,
        NEW_LINE,
        BRACKET,
        QUOTE,
        COMMA
    }
    class MdlFileContent
    {
        public List<string> Content { get; }
        public int Index { get; set; }
        public bool EndOfFile
        {
            get
            {
                return Index == Content.Count ? true : false;
            }
        }
        // static readonly HashSet<char> splitChars = new HashSet<char> { '(', ')' };
        public MdlFileContent(string path)
        {
            Content = new List<string>();
            Index = 0;

            StreamReader stream = new StreamReader(path);
            string s = "";
            char c;
            State state = State.START_UP;
            while (!stream.EndOfStream)
            {
                c = (char)stream.Peek();
                //skip '\r'
                if (c == '\r') { stream.Read(); continue; }
                switch (state)
                {
                    case State.START_UP:
                        if (IsWhiteSpace(c)) { stream.Read(); state = State.EMPTY; }
                        else if (IsNewLine(c)) { s += (char)stream.Read(); state = State.NEW_LINE; }
                        else if (IsBracket(c)) { s += (char)stream.Read(); state = State.BRACKET; }
                        else if (IsQuote(c)) { s += (char)stream.Read(); state = State.QUOTE; }
                        else { s += (char)stream.Read(); state = State.REMAINING; }
                        break;
                    case State.EMPTY:
                        if (IsWhiteSpace(c)) { stream.Read(); }
                        else if (IsNewLine(c)) { s += (char)stream.Read(); state = State.NEW_LINE; }
                        else if (IsBracket(c)) { s += (char)stream.Read(); state = State.BRACKET; }
                        else if (IsQuote(c)) { s += (char)stream.Read(); state = State.QUOTE; }
                        else { s += (char)stream.Read(); state = State.REMAINING; }
                        break;
                    case State.REMAINING:
                        if (IsQuote(c)) { s += (char)stream.Read(); state = State.QUOTE; }
                        else if (IsComma(c)) { s += (char)stream.Read(); state = State.COMMA; }
                        else if (IsWhiteSpace(c) || IsNewLine(c) || IsBracket(c)) { state = State.END_OFF; }
                        else { s += (char)stream.Read(); }
                        break;
                    case State.TEXT:
                        if (IsNewLine(s.Last()) && (IsNewLine(c) || IsWhiteSpace(c))) { state = State.END_OFF; }
                        else { s += (char)stream.Read(); }
                        break;
                    case State.NEW_LINE:
                        if (IsNewLine(c)) { stream.Read(); }
                        else if (c == '|') { s += (char)stream.Read(); state = State.TEXT; }
                        else { state = State.END_OFF; }
                        break;
                    case State.BRACKET:
                        state = State.END_OFF;
                        break;
                    case State.QUOTE:
                        if (IsQuote(c)) { s += (char)stream.Read(); state = State.END_OFF; }
                        else { s += (char)stream.Read(); }
                        break;
                    case State.COMMA:
                        if (IsBracket(c)) { state = State.END_OFF; }
                        else { s += (char)stream.Read(); }
                        break;
                    case State.END_OFF:
                        Content.Add(s);
                        s = "";
                        state = State.START_UP;
                        break;
                }
            }
            Content.Add(s);
        }
        public string Next()
        {
            return Content[Index++];
        }
        public string Prev()
        {
            return Content[Index--];
        }
        public string Peek()
        {
            return Content[Index];
        }
        // StreamReader SkipWhiteSpace(StreamReader stream)
        // {
        //     while (!stream.EndOfStream && IsWhiteSpace((char)stream.Peek())) { stream.Read(); }
        //     return stream;
        // }
        bool IsWhiteSpace(char c)
        {
            if (c == ' ' || c == '\t') return true;
            return false;
        }
        bool IsBracket(char c)
        {
            if (c == '(' || c == ')') return true;
            return false;
        }
        bool IsNewLine(char c)
        {
            if (c == '\n') return true;
            return false;
        }
        bool IsQuote(char c)
        {
            if (c == '\"') return true;
            return false;
        }
        bool IsComma(char c)
        {
            if (c == ',') return true;
            return false;
        }
    }
}
