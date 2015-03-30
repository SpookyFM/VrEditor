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
using System.Windows.Shapes;

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

        private void bChooseFile_Click(object sender, RoutedEventArgs e)
        {
            String filename = String.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            // dialog.Filter = "*.*";
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                (DataContext as Asset).File = dialog.FileName;
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
