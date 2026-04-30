using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EyeshotTableOutput;

public partial class DataColumnStyle
{
    public bool IsVisible {get; set;}
    public int Width {get; set;}
    public bool IsWrapped {get; set;}
    public Text.alignmentType Alignment { get; set; } = Text.alignmentType.MiddleCenter;
}

