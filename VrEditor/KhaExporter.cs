using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VrEditor.json;



namespace VrEditor
{

    

    public class KhaExporter
    {
        public json.Project Project;

        public void BuildProject(IEnumerable<Asset> assets, String folderName)
        {
            Project = new json.Project();
            Project.game = new json.Game();
            Room room = new Room();
            room.id = Guid.NewGuid().ToString();

            Project.rooms.Add(room);

            foreach (Asset a in assets) {
                json.Asset asset = new json.Asset();
                
                // TODO: Relative to the project path!
                asset.file = getRelativePath(folderName, a.File);
                asset.name = a.Name;
                asset.type = a.Type;
                asset.id = Guid.NewGuid().ToString();
                Project.assets.Add(asset);
                room.assets.Add(asset.id);
            }
        }


        private String getRelativePath(String basePath, String filename)
        {
            System.Uri uri1 = new Uri(filename);
            System.Uri uri2 = new Uri(basePath);

            Uri relativeUri = uri2.MakeRelativeUri(uri1);

            return relativeUri.ToString();
        }

        public void SaveTo(String filename)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            String result = serializer.Serialize(Project);
            System.IO.File.WriteAllText(filename, result);
        }

    }
}
