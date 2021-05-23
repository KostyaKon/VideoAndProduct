using System.Xml;
using System.IO;

public class ReadXML
{
    string urlVideo = "";

    /// <summary>
    /// Method accepting a string containing vast.xml and returning an address with a video file
    /// </summary>
    /// <param name="xmlDateString">a string containing vast.xml</param>
    /// <returns></returns>
    public string GetUrlVideo(string xmlDateString)
    {
        var stringRider = new StringReader(xmlDateString);
        var reader = new XmlTextReader(stringRider);

        // пробуем найти ноду MediaFile и вернуть её содержимое
        urlVideo = GetUrl(reader, "MediaFile");
        if (string.IsNullOrEmpty(urlVideo))
        {
            // если вернулась пустая строка или нулл, то возможно vast содержит ссылку на другой vast
            string newVast = GetUrl(reader, "VASTAdTagURI");
            if (!string.IsNullOrEmpty(newVast))
            {
                // если строка не пустая, то рекурсивно запускаем новый vast.xml
                urlVideo = GetUrlVideo(newVast);
            }
            else
                // иначе возвращаем строку "Error"
                urlVideo = "Error";
        }
        reader.Close();
        return urlVideo;
    }

    /// <summary>
    /// A method that accepts an XmlTextReader and a node name, and returns the contents of this node or an empty string
    /// </summary>
    /// <param name="xmlTextReader"></param>
    /// <param name="nameNode"></param>
    /// <returns></returns>
    private string GetUrl(XmlTextReader xmlTextReader, string nameNode)
    {
        string url = "";
        while (xmlTextReader.Read())
        {
            if (xmlTextReader.NodeType == XmlNodeType.Element)
            {
                if (xmlTextReader.Name.Equals(nameNode))
                {
                    url = xmlTextReader.ReadString();
                    return url;
                }
            }
        }
        return url;
    }
}
