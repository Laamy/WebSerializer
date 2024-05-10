
using System.Text;

public class JsonFormatter
{
    public static string Indent(int level, JsonFormatOptions options)
    {
        return new string(' ', level * options.IndentSize);
    }

    public static string FormatJson(string json, JsonFormatOptions options)
    {
        var result = new StringBuilder();
        int indentLevel = 0;
        bool inQuotes = false;
        char currentChar;

        for (int i = 0; i < json.Length; i++)
        {
            currentChar = json[i];

            if (currentChar == '"')
            {
                inQuotes = !inQuotes;
                result.Append(currentChar);
                continue;
            }

            if (!inQuotes)
            {
                switch (currentChar)
                {
                    case '{':
                    case '[':
                        result.Append(currentChar);
                        result.AppendLine();
                        indentLevel++;
                        result.Append(Indent(indentLevel, options));
                        break;
                    case '}':
                    case ']':
                        result.AppendLine();
                        indentLevel--;
                        result.Append(Indent(indentLevel, options));
                        result.Append(currentChar);
                        break;
                    case ',':
                        result.Append(currentChar);
                        result.AppendLine();
                        result.Append(Indent(indentLevel, options));
                        break;
                    default:
                        result.Append(currentChar);
                        break;
                }
            }
            else
            {
                result.Append(currentChar);
            }
        }

        return result.ToString();
    }
}