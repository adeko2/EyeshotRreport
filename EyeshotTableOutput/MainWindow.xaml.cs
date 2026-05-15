using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System.Windows;
using System.Windows.Controls;

namespace EyeshotTableOutput;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonDrawTable_Click(object sender, RoutedEventArgs e)
    {
        drawing1.TextStyles["Default"].FontFamilyName = "Arial";
        drawing1.Blocks.Clear();
        Sheet sheet = AddNewDrawingSheet();
        try
        {
            Block tableBlock = new("TableBlock");
            PageItemTableDrawer drawer = new PageItemTableDrawer(tableBlock);
            BlockReference br = drawer.Draw();
            sheet.Entities.Add(br);
            drawing1.Blocks.Add(tableBlock);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "exception!", MessageBoxButton.OK);
            return;
        }
        // it is strictly necessary to set the active sheet
        // after all the items have been added, otherwise the sheet is not displayed
        drawing1.ActiveSheet = sheet;
        drawing1.Rebuild(design1);
        tabControl.SelectedIndex = 1;        
        drawing1.Refresh();
        drawing1.ZoomFit();
    }

    private void btnPrintPreview_Click(object sender, RoutedEventArgs e)
    {
        drawing1.PrintPreview(new(450, 400), true);
    }
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
        drawing1.Print(true);
    }

    private void btnPageSetup_Click(object sender, RoutedEventArgs e)
    {
        drawing1.PageSetup(true);
    }

    private Sheet AddNewDrawingSheet()
    {
        drawing1.ActiveSheet = null;
        drawing1.Sheets.Clear();
        var size = IsoPageSize.By(SheetFormat.A4_ISO);
        var sheet = new Sheet(linearUnitsType.Millimeters, size.Width, size.Height, "Sheet 1", angleProjectionType.FirstAngle);
        Block blockA4;
        //BlockReference br = sheet.BuildA4ISO(out blockA4, null);
        BlockReference br = SheetBlockReference.GetSheetBlock(size, "blabla", out blockA4);
        drawing1.Sheets.Add(sheet);
        drawing1.Blocks.Add(blockA4);
        sheet.Entities.Add(br);
        return sheet;
    }
}