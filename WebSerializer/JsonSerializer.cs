using System;
using System.Collections;
using System.Reflection;
using System.Text;

public class JsonSerializer
{
    public string Serialize(object obj, JsonFormatOptions options = null)
    {

        StringBuilder sb = new StringBuilder();
        SerializeValue(obj, sb);

        if (options != null)
            return JsonFormatter.FormatJson(sb.ToString(), options);

        return sb.ToString();
    }

    private void SerializeValue(object value, StringBuilder sb)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else if (value is string)
        {
            SerializeString((string)value, sb);
        }
        else if (value is bool)
        {
            sb.Append(((bool)value) ? "true" : "false");
        }
        else if (value is ValueType || value is Enum)
        {
            sb.Append(value.ToString().ToLower());
        }
        else if (value is IDictionary dict)
        {
            SerializeDictionary(dict, sb);
        }
        else if (value is IEnumerable enumerable)
        {
            SerializeEnumerable(enumerable, sb);
        }
        else
        {
            SerializeObject(value, sb);
        }
    }

    private void SerializeString(string value, StringBuilder sb)
    {
        sb.Append("\"");
        foreach (char c in value)
        {
            switch (c)
            {
                case '"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        sb.Append("\"");
    }

    private void SerializeDictionary(IDictionary dict, StringBuilder sb)
    {
        sb.Append("{");
        bool first = true;
        foreach (DictionaryEntry entry in dict)
        {
            if (!first)
            {
                sb.Append(",");
            }
            SerializeString(entry.Key.ToString(), sb);
            sb.Append(":");
            SerializeValue(entry.Value, sb);
            first = false;
        }
        sb.Append("}");
    }

    private void SerializeEnumerable(IEnumerable enumerable, StringBuilder sb)
    {
        sb.Append("[");
        bool first = true;
        foreach (var item in enumerable)
        {
            if (!first)
            {
                sb.Append(",");
            }
            SerializeValue(item, sb);
            first = false;
        }
        sb.Append("]");
    }

    private void SerializeObject(object obj, StringBuilder sb)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        sb.Append("{");
        bool first = true;
        foreach (PropertyInfo property in properties)
        {
            if (!first)
            {
                sb.Append(",");
            }
            SerializeString(property.Name, sb);
            sb.Append(":");
            SerializeValue(property.GetValue(obj), sb);
            first = false;
        }
        sb.Append("}");
    }
}