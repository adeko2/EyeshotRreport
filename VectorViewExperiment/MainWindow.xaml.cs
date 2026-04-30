using CabinCAD.Layout;
using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Control.Labels;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Geometry.ConstraintSolver;
using devDept.Serialization;
using Microsoft.Win32;
using ODA.Kernel.TD_RootIntegrated;
using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrawingRequiresVectorView;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public string _text = string.Empty;
    public string LabelText
    {
        get => _text;
        set
        {
            if (value != _text)
            {
                _text = value;
                if (_view is not null)
                {
                    _view.TextString = _text;
                    RefreshDrawing();

                }
                NotifyPropertyChange();
            }
        }
    }

    private void RefreshDrawing()
    {
        drawing1.Entities.Remove(_br);
        //drawing1.Rebuild(design1, changedOnly:true);
        drawing1.Entities.Add(_br);
        //drawing1.Rebuild(design1, changedOnly: true);
        drawing1.Refresh();
    }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChange([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        //LoadFile();
        //design1.Refresh();
        LoadBoxes();
    }


    public BlockReference? GetView(Block block)
    {
        _view = new(0, -tH / 2, LabelText, tW, 2, 2.5)
        {
            RectHeight = tH,
            Wrap = true,
        };
        _border = new(0, 0, tW, -tH);
        block.Entities.Add(_view);
        block.Entities.Add(_border);
        _br = new(0, 0, 0d, block.Name, 0d);
        return _br;
    }

    private void LoadFile()
    {
        OpenFileDialog dialog = new ();
        bool? res = dialog.ShowDialog();
        if(res == true && !string.IsNullOrWhiteSpace(dialog.FileName))
        {
            LoadFromEye(design1, dialog.FileName);
        }
    }

    private static void LoadFromEye(Design design, string filePath)
    {
        using (FileStream fs = File.Open(filePath, FileMode.Open))
        {
            FileSerializer serialzier = new();
            design.OpenFile(fs, fileSerializer: serialzier);
        }
        design.ActiveViewport.Rotate.RotationMode = rotationType.Turntable;
        design.Entities.Regen();
        design.Invalidate();
        design.ZoomFit();
    }

    private void LoadBoxes()
    {
        var box = Brep.CreateBox(10, 10, 10, 10);
        Text txt = new Text(Point3D.Origin, "1", 10, Text.alignmentType.MiddleCenter)
        {
            Billboard = true
        };
        LeaderAndText lbl = new(Point3D.Origin, "2", new Font("Arial", 10), Color.Black, 100, 100);
        WireBox wb = new(0, 0, 0, 20, 20, 20);
        Block b1 = new("b1");
        b1.Entities.Add(wb);
        b1.Entities.Add(box, Color.Green);
        b1.Entities.Add(txt, Color.Blue);
        InsertBase b1Ins = new(b1.Name, 1);
        design1.Blocks.Add(b1);
        design1.Entities.Add(b1Ins, Color.Red);
        design1.Labels.Add(lbl);
        design1.ActiveViewport.DisplayMode = displayType.Shaded;
        design1.ActiveViewport.Rotate.RotationMode = rotationType.Turntable;
        design1.Refresh();


        Table table = new(Plane.XY, 2, 2, 10, 20, 2, Table.flowDirection.Down);
        var tableBlock = new Block("tableBlock");
        tableBlock.Entities.Add(table);
        InsertBase tableBr = new(tableBlock.Name, 0);

        Block textBlock = new("tBlock");
        BlockReference? tBR = GetView(textBlock);

        var size = IsoPageSize.By(SheetFormat.A4_ISO);
        var sheet1 = new Sheet(linearUnitsType.Millimeters, size.Width, size.Height, "Sheet 1", angleProjectionType.FirstAngle);
        Block blockA4;
        BlockReference br = SheetBlockReference.GetSheetBlock(size, "blabla", out blockA4);
        drawing1.Sheets.Add(sheet1);
        drawing1.Blocks.Add(blockA4);
        sheet1.Entities.Add(br);

        if (tBR is not null)
        {
            drawing1.Blocks.Add(textBlock);
            sheet1.Entities.Add(tBR);
        }

        drawing1.ActiveSheet = sheet1;

        drawing1.Blocks.Add(tableBlock);
        sheet1.Entities.Add(tableBr);

        sheet1.Rebuild(design1, drawing1);
        //drawing1.Invalidate();
        FileSerializer ser = new();
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "eye_test.eye");
        using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write))
            design1.SaveFile(fs);
    }

    double tW = 40;
    double tH = 20;
    private MultilineText? _view;
    BlockReference? _br;
    LinearPath? _border;
}