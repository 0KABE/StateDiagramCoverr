using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StateMachine.MdlReader
{
    class LocatedElement
    {
        public string Location { get; set; }
        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    abstract class Element : LocatedElement { }
    class Position : Element
    {
        public long X { get; set; }
        public long Y { get; set; }
    }
    class MDLFile : LocatedElement
    {
        List<Element> elements = new List<Element>();
        public List<Element> Elements { get { return elements; } }
    }
    class TypeElement : Element
    {
        public string Type { get; set; }
    }
    class ValueElement : TypeElement
    {
        public Value Value { get; set; }
    }
    class List : TypeElement
    {
        List<Element> elements = new List<Element>();
        public List<Element> Elements { get { return elements; } }
    }
    class Object : TypeElement
    {
        List<Property> properties = new List<Property>();
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Id { get; set; }
        public List<Property> Properties { get { return properties; } }
    }
    class Property : LocatedElement
    {
        public string Name { get; set; }
        public Value Value { get; set; }
    }
    abstract class Value : LocatedElement { }
    class DoubleVal : Value
    {
        public double Value { get; set; }
    }
    class IntegerVal : Value
    {
        public int Value { get; set; }
    }
    class StringVal : Value
    {
        public string Value { get; set; }
    }
    class BooleanVal : Value
    {
        public bool Value { get; set; }
    }
    class IdVal : Value
    {
        public int Value { get; set; }
    }
    class EnumVal : Value
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    class ElementVal : Value
    {
        public Element Value { get; set; }
    }
    class TextVal : Value
    {
        public string Value { get; set; }
    }
}
