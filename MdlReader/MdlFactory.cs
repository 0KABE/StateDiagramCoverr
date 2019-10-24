using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace StateMachine.MdlReader
{
    interface IElementFactory
    {
        Element GetElement(MdlFileContent file);
    }
    class ElementFactory : IElementFactory
    {
        static readonly Regex regex = new Regex(@"[\d\s]*,[\d\s]*");
        public Element GetElement(MdlFileContent file)
        {
            IElementFactory elementFactory;
            while (!file.EndOfFile && string.IsNullOrWhiteSpace(file.Peek())) file.Next();
            if (file.Content[file.Index] == "(" && regex.IsMatch(file.Content[file.Index + 1])) elementFactory = new PositionFactory();
            else elementFactory = new TypeElementFactory();
            return elementFactory.GetElement(file);
        }
    }
    class PositionFactory : IElementFactory
    {
        public Element GetElement(MdlFileContent file)
        {
            file.Next();
            string[] p = file.Next().Split(',');
            Position position = new Position();
            position.X = long.Parse(p[0]);
            position.Y = long.Parse(p[1]);
            file.Next();
            return position;
        }
    }
    interface ITypeElementFactory
    {
        TypeElement GetTypeElement(MdlFileContent file);
    }
    class TypeElementFactory : IElementFactory, ITypeElementFactory
    {
        public Element GetElement(MdlFileContent file) { return GetTypeElement(file); }
        public TypeElement GetTypeElement(MdlFileContent file)
        {
            file.Next();
            string s = file.Next();
            ITypeElementFactory typeElementFactory;
            if (s.StartsWith("object")) typeElementFactory = new ObjectFactory();
            else if (s.StartsWith("list")) typeElementFactory = new ListFactory();
            else typeElementFactory = new ValueElementFactory();
            TypeElement typeElement = typeElementFactory.GetTypeElement(file);
            file.Next();
            return typeElement;
        }
    }
    class ObjectFactory : ITypeElementFactory
    {
        public TypeElement GetTypeElement(MdlFileContent file)
        {
            Object obj = new Object();
            obj.Type = file.Next();
            IValueFactory valueFactory = new ValueFactory();
            if (file.Peek() != "\n")
            {
                obj.Name = file.Next();
                if (file.Peek() != "\n")
                {
                    obj.SecondName = file.Next();
                }
            }
            file.Next();
            while (file.Peek() != ")")
            {
                while (string.IsNullOrWhiteSpace(file.Peek())) file.Next();
                Property property = new Property();
                property.Name = file.Next();
                property.Value = valueFactory.GetValue(file);
                obj.Properties.Add(property);
            }
            return obj;
        }
    }
    class ValueElementFactory : ITypeElementFactory
    {
        public TypeElement GetTypeElement(MdlFileContent file)
        {
            ValueFactory valueFactory = new ValueFactory();
            ValueElement valueElement = new ValueElement();
            valueElement.Value = valueFactory.GetValue(file);
            return valueElement;
        }
    }
    class ListFactory : ITypeElementFactory
    {
        public TypeElement GetTypeElement(MdlFileContent file)
        {
            IElementFactory elementFactory = new ElementFactory();
            StateMachine.MdlReader.List list = new List();
            list.Type = file.Next();
            while (file.Peek() != ")")
            {
                list.Elements.Add(elementFactory.GetElement(file));
            }
            return list;
        }
    }
    interface IValueFactory
    {
        Value GetValue(MdlFileContent file);
    }
    class ValueFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            int intResult;
            double doubleResult;
            bool boolResult;
            int index = file.Index;

            IValueFactory valueFactory;
            if (file.Content[index] == "("
                && file.Content[index + 1].StartsWith("\"")
                && int.TryParse(file.Content[index + 2], out intResult)
                && file.Content[index + 3] == ")")
                valueFactory = new EnumValFactory();
            else if (file.Content[index] == "("
                && file.Content[index + 1] == "value"
                && file.Content[index + 2] == "Text")
                valueFactory = new TextValFactory();
            else if (int.TryParse(file.Peek(), out intResult)) valueFactory = new IntegerValFactory();
            else if (double.TryParse(file.Peek(), out doubleResult)) valueFactory = new DoubleValFactory();
            else if (bool.TryParse(file.Peek(), out boolResult)) valueFactory = new BooleanValFactory();
            else if (file.Peek().StartsWith("\"") || file.Peek().StartsWith("\n|")) valueFactory = new StringValFactory();
            else if (file.Peek().StartsWith("@")) valueFactory = new IdValFactory();
            else valueFactory = new ElementValFacotory();
            return valueFactory.GetValue(file);
        }
    }
    class EnumValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            file.Next();
            EnumVal enumVal = new EnumVal();
            enumVal.Name = file.Next();
            enumVal.Value = int.Parse(file.Next());
            file.Next();
            return enumVal;
        }
    }
    class IdValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            IdVal idVal = new IdVal();
            idVal.Value = int.Parse(file.Next().TrimStart('@'));
            return idVal;
        }
    }
    class BooleanValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            BooleanVal booleanVal = new BooleanVal();
            booleanVal.Value = bool.Parse(file.Next());
            return booleanVal;
        }
    }
    class StringValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            StringVal stringVal = new StringVal();
            stringVal.Value = file.Next();
            return stringVal;
        }
    }
    class IntegerValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            IntegerVal integerVal = new IntegerVal();
            integerVal.Value = int.Parse(file.Next());
            return integerVal;
        }
    }
    class DoubleValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            DoubleVal doubleVal = new DoubleVal();
            doubleVal.Value = double.Parse(file.Next());
            return doubleVal;
        }
    }
    class ElementValFacotory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            IElementFactory elementFactory = new ElementFactory();
            ElementVal elementVal = new ElementVal();
            elementVal.Value = elementFactory.GetElement(file);
            return elementVal;
        }
    }
    class TextValFactory : IValueFactory
    {
        public Value GetValue(MdlFileContent file)
        {
            TextVal textVal = new TextVal();
            int bracketCount = 0;
            file.Next(); file.Next(); file.Next();
            while (bracketCount > 0 || file.Peek() != ")")
            {
                if (file.Peek() == "(") ++bracketCount;
                else if (file.Peek() == ")") --bracketCount;
                textVal.Value += file.Next();
            }
            file.Next();
            return textVal;
        }
    }
}
