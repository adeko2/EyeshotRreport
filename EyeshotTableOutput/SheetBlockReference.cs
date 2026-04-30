using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeshotTableOutput;

public class SheetBlockReference : BlockReference
{
    public SheetBlockReference(BlockReference reference) : base(reference.BlockName)
    {
    }
    public SheetBlockReference(string blockName) : base(blockName)
    {
    }
    public static SheetBlockReference GetSheetBlock(Size size, string id, out Block sheetBlock)
    {
        string blockName = $"sheet_{id}";
        sheetBlock = new(blockName);
        SheetBlockReference sheetBlockRef = new(blockName);
        return sheetBlockRef;
    }

}
