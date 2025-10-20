using System.Text;
using System.Text.Json;
using WzComparerR2.WzLib;
using WzPipeline.Core.Stereotype;
using WzPipeline.Shared.Node;

namespace WzPipeline.Shared.Converter;

public class NodeJsonConverter(string path) : AbstractConverter<WzNode, KeyedString>
{
    private string[] _paths = path.Split("/");
    
    public override KeyedString Convert(WzNode node)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        WriteNode(node.Node, writer, 0, false);
        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());

        return new KeyedString
        {
            Key = node.Node.Text,
            Value = json
        };
    }

    private void WriteNode(Wz_Node node, Utf8JsonWriter writer, int index, bool property = true)
    {
        if (index > 0 && index - 1 < _paths.Length && node.Text != _paths[index - 1])
        {
            return;
        }
        
        var value = node.Value;

        if (property)
        {
            writer.WritePropertyName(node.Text);
        }

        if (value == null || value is Wz_Image)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue("@dir");
            }
        }
        else if (value is Wz_Png png)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WriteString("$", "png");

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue("@png");
            }
        }
        else if (value is Wz_Uol uol)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WriteString("@uol", uol.Uol);

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue($"@uol:{uol.Uol}");
            }
        }
        else if (value is Wz_Vector vector)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@vector");
                writer.WriteStartArray();
                writer.WriteNumberValue(vector.X);
                writer.WriteNumberValue(vector.Y);
                writer.WriteEndArray();

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(vector.X);
                writer.WriteNumberValue(vector.Y);
                writer.WriteEndArray();
            }
        }
        else if (value is Wz_Sound sound)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WriteString("$", "sound");

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue("@sound");
            }
        }
        else if (value is Wz_Convex convex)
        {
            if  (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@convex");
                writer.WriteStartArray();
                foreach (var point in convex.Points)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(point.X);
                    writer.WriteNumberValue(point.Y);
                    writer.WriteEndArray();
                }
                writer.WriteEndArray();
                
                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray();
                foreach (var point in convex.Points)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(point.X);
                    writer.WriteNumberValue(point.Y);
                    writer.WriteEndArray();
                }
                writer.WriteEndArray();
            }
        }
        else if (value is Wz_RawData rawData)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WriteString("$", "raw");
                writer.WriteNumber("@length", rawData.Length);

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue($"@raw:length={rawData.Length}");
            }
        }
        else if (value is Wz_Video video)
        {
            if (node.Nodes.Count > 0)
            {
                writer.WriteStartObject();
                writer.WriteString("$", "video");
                writer.WriteNumber("@length", video.Length);

                foreach (var child in node.Nodes)
                {
                    WriteNode(child, writer, index + 1);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue($"@video:length={video.Length}");
            }
        }
        else
        {
            if (value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal)
            {
                writer.WriteNumberValue(System.Convert.ToDouble(value));
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}