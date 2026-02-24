using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;
using ProtoBuf.Meta;

namespace DrawingRequiresVectorView;

public class InsertBaseSurrogate : BlockReferenceSurrogate
{
    public int Pose { get; set; }

    public InsertBaseSurrogate(InsertBase obj) : base(obj)
    {
    }

    protected override Entity ConvertToObject()
    {
        InsertBase ins = new("", 0);
        CopyDataToObject(ins);
        return ins;
    }
    protected override void CopyDataFromObject(Entity obj)
    {
        base.CopyDataFromObject(obj);
        if (obj is InsertBase ins)
        {
            Pose = ins.Pose;
        }
    }

    protected override void CopyDataToObject(Entity obj)
    {
        if (obj is BlockReference br)
        {
            // base class does not copy BlockName. Weird but it is.
            // BlockName is copied when base.ConvertToObject() calls c'tor of BlockReference
            br.BlockName = BlockName;
            base.CopyDataToObject(br);
        }

        if (obj is InsertBase ins)
        {
            ins.Pose = Pose;
        }
    }

    // implicit covertors
    public static implicit operator InsertBase(InsertBaseSurrogate surrogate)
    {
        return (surrogate?.ConvertToObject() as InsertBase)!;
    }
    public static implicit operator InsertBaseSurrogate(InsertBase source)
    {
        return (source?.ConvertToSurrogate() as InsertBaseSurrogate)!;
    }

    /// <summary>
    /// requires:
    /// <list type="bullet">
    /// <item><see cref="AdeDataSurrogate"/></item>
    /// </list>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="assertRequireSurrogateAdded"></param>
    public static void AddSurrogateToModel(RuntimeTypeModel model)
    {
        model[typeof(BlockReference)].AddSubType(1001, typeof(InsertBase));
        model[typeof(BlockReferenceSurrogate)].AddSubType(1001, typeof(InsertBaseSurrogate));

        model[typeof(InsertBase)].SetSurrogate(typeof(InsertBaseSurrogate));
        model[typeof(InsertBaseSurrogate)]
            .Add(1, nameof(Pose))
            .SetCallbacks(null, null, "BeforeDeserialize", null)
            .UseConstructor = false;
    }
}