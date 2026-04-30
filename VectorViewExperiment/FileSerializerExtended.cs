using devDept.Eyeshot.Entities;
using devDept.Serialization;
using DrawingRequiresVectorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorViewExperiment;

internal class FileSerializerExtended : FileSerializer
{
    protected override void FillModel()
    {
        if (ModelIsCompiled()) return;

        base.FillModel();

    }
}
