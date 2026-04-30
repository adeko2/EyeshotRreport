using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PresentationAppearanceResource;

namespace EyeshotTableOutput;

public enum SheetFormat
{
    A0_ISO,
    A1_ISO,
    A2_ISO,
    A3_ISO,
    A4_ISO,
    A4_LANDSCAPE_ISO,
}


/// <summary>
/// Page sizes according to ISO 216 standard in millimeters.
/// </summary>
public static class IsoPageSize
{
    /// <summary>
    /// Millimeters
    /// </summary>
    public static linearUnitsType Units => linearUnitsType.Millimeters;

    public static readonly Size A0 = new(841, 1189);
    public static readonly Size A1 = new(594, 841);
    public static readonly Size A2 = new(420, 594);
    public static readonly Size A3 = new(297, 420);
    public static readonly Size A4 = new(210, 297);
    public static readonly Size A5 = new(148, 210);

    public static Size By(SheetFormat format)
    {
        switch (format)
        {
            case SheetFormat.A0_ISO:
                return A0;
            case SheetFormat.A1_ISO:
                return A1;
            case SheetFormat.A2_ISO:
                return A2;
            case SheetFormat.A3_ISO:
                return A3;
            case SheetFormat.A4_ISO:
                return A4;
            case SheetFormat.A4_LANDSCAPE_ISO:
                return new(A4.Height, A4.Width);
            default:
                throw new InvalidOperationException($"The sheet format {format} has not been defined in TechnicalDrawing.");
        }
    }
}
