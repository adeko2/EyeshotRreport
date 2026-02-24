using devDept.Eyeshot.Entities;
using devDept.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrawingRequiresVectorView;

public class InsertBase : BlockReference, ISerializable
{
    public int Pose { get; set; } = -1;

    protected InsertBase(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Pose = (int)info.GetValue("Pose", typeof(int));
    }

    public InsertBase(string blockName, int poose) : base(blockName)
    {
        Pose = poose;
    }

    protected InsertBase(BlockReference blockReference, bool keepTessellation = false) : base(blockReference, keepTessellation)
    {

    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Pose", Pose);
    }

    public override object CloneWithTessellation()
    {
        // from eyeshot source code:
        InsertBase newInsert = new(this, RegenMode != regenType.RegenAndCompile);
        CopyDataTo(newInsert);
        return newInsert;
    }

    public override object Clone()
    {
        InsertBase newInsert = new(this);
        CopyDataTo(newInsert);
        return newInsert;
    }

    protected virtual void CopyDataTo(InsertBase insert)
    {
        insert.Pose = Pose;
    }

    public new InsertBaseSurrogate ConvertToSurrogate()
    {
        return new InsertBaseSurrogate(this);
    }
}
