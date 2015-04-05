using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace VrEditor
{
    /// <summary>
    /// Interaction logic for AssetEditor.xaml
    /// </summary>
    public partial class AssetEditor : Window
    {
        public AssetEditor()
        {
            InitializeComponent();
        }


        public static void FromFile(String filename, Asset asset)
        {
            List<String> imageExtensions = new List<string>();
            imageExtensions.Add(".jpg");
            imageExtensions.Add(".jpeg");
            imageExtensions.Add(".png");

            List<String> musicExtensions = new List<string>();
            musicExtensions.Add(".mp3");

            List<String> soundExtensions = new List<string>();
            soundExtensions.Add(".wav");

            String extension = System.IO.Path.GetExtension(filename);
            String name = System.IO.Path.GetFileNameWithoutExtension(filename);

            asset.File = filename;

            // Fill the name if it is still empty
            if (asset.Name == "New Asset")
            {
                asset.Name = name;
            }

            // Fill the type if we recognize it
            if (imageExtensions.Contains(extension))
            {
                asset.Type = "image";
            }
            else if (musicExtensions.Contains(extension))
            {
                asset.Type = "music";
            }
            else if (soundExtensions.Contains(extension))
            {
                asset.Type = "sound";
            }
            else
            {
                asset.Type = "blob";
            }
        }

        private void InitializeFromFilename(String filename)
        {
            FromFile(filename, DataContext as Asset);
        }

        private void bChooseFile_Click(object sender, RoutedEventArgs e)
        {
            String filename = String.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            // dialog.Filter = "*.*";
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                (DataContext as Asset).File = dialog.FileName;
                InitializeFromFilename(dialog.FileName);
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
