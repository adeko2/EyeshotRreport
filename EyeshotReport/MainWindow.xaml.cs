using devDept.Eyeshot;
using devDept.Serialization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace EyeshotBugReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _bugEntitiFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oven.pbf");
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            LoadEntities();
            design1.ZoomFit();
            design1.Invalidate();
        }

        private void LoadEntities()
        {
            FileSerializer serializer = new();
            using (FileStream fs = File.Open(_bugEntitiFile, FileMode.Open))
            {
                serializer.Deserialize(fs);
            }

            var blocks = serializer.FileBody.Blocks;
            var entities = serializer.FileBody.Entities;
            foreach (var entity in entities)
            {
                if (!design1.Layers.Contains(entity.LayerName))
                {
                    design1.Layers.Add(entity.LayerName, System.Drawing.Color.White);
                }
                entity.MaterialName = null;
            }
            foreach(Block block in blocks)
            {
                foreach (var entity in block.Entities)
                {
                    if (!design1.Layers.Contains(entity.LayerName))
                    {
                        design1.Layers.Add(entity.LayerName);
                    }
                    entity.MaterialName = null;
                }
            }
            design1.Blocks.AddRange(blocks);
            design1.Entities.AddRange(entities);
        }
    }
}