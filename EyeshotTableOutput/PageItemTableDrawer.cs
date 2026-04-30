using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;

namespace EyeshotTableOutput;

internal readonly record struct TableRenderSettings(double RowHeight, double ColumnWidth, double TextHeight);

internal class PageItemTableDrawer
{
    private readonly Block _block;

    private int X => 20;
    private int Y => 250;
    private double RowHeight => 6;
    private double ColumnWidth => 30;
    private double TextHeight => 4;
    private double VerticalCellMargin => 1;
    private double HorizontalCellMargin => 1;
    private List<DataColumnStyle> DataColumnStyles { get; } = new List<DataColumnStyle>()
    {
        new (){
            IsVisible = false,
            IsWrapped = false,
            Alignment = Text.alignmentType.MiddleLeft,
            Width = 40
        },
        new (){
            IsVisible = true,
            IsWrapped = false,
            Alignment = Text.alignmentType.MiddleLeft,
            Width = 40
        },
        new (){
            IsVisible = true,
            IsWrapped = false,
            Alignment = Text.alignmentType.MiddleLeft,
            Width = 40
        },
        new (){
            IsVisible = true,
            IsWrapped = false,
            Alignment = Text.alignmentType.MiddleLeft,
            Width = 40
        },
    };

    internal PageItemTableDrawer(Block block)
    {
        _block = block;
    }

    internal BlockReference Draw()
    {
        TableRenderSettings settings = new TableRenderSettings(RowHeight, ColumnWidth, TextHeight);

        var data = RetrieveData();
        var view = GenerateView(data, settings);
        _block.Entities.Add(view);
        BlockReference br = new(X, Y, 0, _block.Name, 0);
        return br;
    }

    private Table GenerateView(DataTable data, TableRenderSettings settings)
    {
        Table table = FillEyeshotTable(data, settings);
        return table;
    }

    private DataTable RetrieveData()
    {
        DataTable data = new();
        data.Columns.Add("Id", typeof(int));
        data.Columns.Add("Name", typeof(string));
        data.Columns.Add("Material", typeof(string));
        data.Columns.Add("type", typeof(string));

        data.Rows.Add(1, "Part 1", "Steel", "BACK_PANEL");
        data.Rows.Add(2, "Part 2", "Wood", "Panel");
        data.Rows.Add(3, "Part 3", "Wood", "Drawer");
        data.Rows.Add(4, "Part 4", "Wood", "panel");
        data.Rows.Add(5, "Part 5", "Wood", "panel");
        data.Rows.Add(6, "Part 6", "Wood", "panel");
        data.Rows.Add(7, "Part 7", "Wood", "panel");
        data.Rows.Add(8, "Part 8", "Wood", "Panel");
        data.Rows.Add(1, "Part 9", "Steel", "back_panel");
        data.Rows.Add(1, "Part 10", "Steel", "back_panel");
        data.Rows.Add(1, "Part 11", "Steel", "back_panel");

        return data;
    }

    private Table FillEyeshotTable(DataTable sourceTable, TableRenderSettings settings)
    {
        List<DataColumn> allDataColumns = new();
        foreach (DataColumn col in sourceTable.Columns)
        {
            allDataColumns.Add(col);
        }

        var visibleColumns = allDataColumns
            .Select((col, i) => (col, style: i < DataColumnStyles.Count ? DataColumnStyles[i] : null))
            .Where(x => x.style?.IsVisible ?? true)
            .ToList();

        List<DataColumn> dataColumns = [.. visibleColumns.Select(vc => vc.col)];

        int numWidths = visibleColumns.Count;
        int totalRows = sourceTable.Rows.Count;
        double[] heights = new double[totalRows];

        for (int i = 0; i < totalRows; i++) heights[i] = settings.RowHeight;

        double[] widths = new double[numWidths];
        for (int i = 0; i < numWidths; i++)
        {
            var style = visibleColumns[i].style;
            widths[i] = style != null && style.Width > 0 ? style.Width : settings.ColumnWidth;
        }

        Table table = new(Plane.XY, totalRows, numWidths, heights, widths, settings.TextHeight, Table.flowDirection.Down)
        {
            LineWeightMethod = colorMethodType.byEntity,
            LineWeight = 0.1f,
            HorCellMargin = HorizontalCellMargin,
            VerCellMargin = VerticalCellMargin
        };

        TransferDataFrom(sourceTable, table, dataColumns);
        SetStyles(table, 0, totalRows);
        return table;
    }


    private void TransferDataFrom(DataTable sourceTable, Table table, List<DataColumn> dataColumns)
    {
        int rowIndex = 0;
        foreach (DataRow row in sourceTable.Rows)
        {
            AddRowDataFrom(row, rowIndex, table, dataColumns);
            rowIndex++;
        }
    }
    private void AddRowDataFrom(DataRow row, int rowIndex, Table table, List<DataColumn> dataColumns)
    {
        int i = 0;
        foreach (var col in dataColumns)
        {
            table.SetTextHeight(rowIndex, i, TextHeight);
            string? txt = row[col.ColumnName].ToString();
            table.SetTextString(rowIndex, i, txt ?? "");
            i++;
        }
    }

    private void SetStyles(Table table, int startRow, int rowCount)
    {
        for (int r = startRow; r < startRow + rowCount; r++)
        {
            int col = 0;
            foreach (DataColumnStyle columnStyle in DataColumnStyles)
            {
                if (columnStyle.IsVisible)
                {
                    table.SetWrap(r, col, columnStyle.IsWrapped);
                    table.SetAlignment(r, col, columnStyle.Alignment);
                    table.SetLineSpaceDistance(r, col, 2);
                    col++;
                }
            }
        }
    }
}
