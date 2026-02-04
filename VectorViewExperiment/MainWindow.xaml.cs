using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;

namespace DrawingRequiresVectorView;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        var box = Brep.CreateBox(10, 10, 10, 10);
        design1.Entities.Add(box, Color.FromArgb(255,100,100,250));
        design1.Refresh();

        var sheet1 = new Sheet(linearUnitsType.Millimeters, 210, 297, "Sheet 1", angleProjectionType.FirstAngle);
        Block blockA4;
        BlockReference br = sheet1.BuildA4ISO(out blockA4, "A4_ISO");
        drawing1.Blocks.Add(blockA4);
        drawing1.Sheets.Add(sheet1);
        sheet1.Entities.Add(br);
        sheet1.Rebuild(design1, drawing1);
        drawing1.ActiveSheet = sheet1;
        drawing1.Invalidate();
    }
}