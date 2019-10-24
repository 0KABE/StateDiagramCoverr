using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StateMachine.MdlReader
{
    class MdlParser
    {
        private string path;
        public MdlParser(string MdlFilePath)
        {
            path = MdlFilePath;
        }
        public MDLFile Parse()
        {
            IElementFactory elementFactory = new ElementFactory();
            MdlFileContent mdlFileContent = new MdlFileContent(path);
            MDLFile file = new MDLFile();
            while (!mdlFileContent.EndOfFile)
            {
                if (string.IsNullOrWhiteSpace(mdlFileContent.Peek())) { mdlFileContent.Next(); continue; }
                file.Elements.Add(elementFactory.GetElement(mdlFileContent));
            }
            return file;
        }
    }
}
