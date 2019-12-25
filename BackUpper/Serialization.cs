using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BackUpper
{
    public class Serialization
    {

        public string SerializeVariables(ControlsWindow.VariablesToSave vars)
        {
            StringBuilder output = new StringBuilder();
            var writer = new StringWriter(output);

            XmlSerializer serializer = new XmlSerializer(typeof(ControlsWindow.VariablesToSave));

            serializer.Serialize(writer, vars);

            return output.ToString();
        }

        public ControlsWindow.VariablesToSave DeserializeVariables(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ControlsWindow.VariablesToSave));
            
            StreamReader reader = new StreamReader(path);
            var loadedVariables = (ControlsWindow.VariablesToSave) serializer.Deserialize(reader);
            reader.Close();

            return loadedVariables;
        }


    }
}