﻿using System;
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

                asset.file = Path.GetFileName(a.File);
                if (a.Type == "music")
                {
                    asset.file = Path.GetFileNameWithoutExtension(a.File);           
                }
                if (a.Type == "sound")
                {
                    asset.file = Path.GetFileNameWithoutExtension(a.File);
                }
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

        private json.Asset GenerateImageAsset(String filename, String folderName)
        {
            json.Asset asset;
            asset = new json.Asset();
            asset.id = Guid.NewGuid().ToString();
            asset.name = Path.GetFileNameWithoutExtension(filename);
            asset.type = "image";
            asset.file = Path.GetFileName(filename);

            return asset;
        }

        public void AddAssets(Game CurrentGame, String folderName)
        {
            foreach (Scene scene in CurrentGame.Scenes)
            {
                json.Asset asset = GenerateImageAsset((string) scene.BackgroundImage, folderName);
                Project.assets.Add(asset);
                Project.rooms[0].assets.Add(asset.id);
            }

            foreach (InventoryItem item in CurrentGame.InventoryItems)
            {
                json.Asset image = GenerateImageAsset((string)item.Image, folderName);
                Project.assets.Add(image);
                Project.rooms[0].assets.Add(image.id);

                json.Asset activeImage = GenerateImageAsset((string)item.ActiveImage, folderName);
                Project.assets.Add(activeImage);
                Project.rooms[0].assets.Add(activeImage.id);

            }
        }

        
    }
}
