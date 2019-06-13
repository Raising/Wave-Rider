

using System;

[AttributeUsage(AttributeTargets.All)]
public class ClassReferenceAttribute : Attribute
{
    public static readonly ClassReferenceAttribute Default = new ClassReferenceAttribute();
    private Type type;

    public ClassReferenceAttribute() : this(typeof(string))
    {
    }
        
    public ClassReferenceAttribute(Type type)
    {
        this.type = type;
    }

    public virtual Type ClassReference { get { return ClassReferenceValue; } }
    protected Type ClassReferenceValue { get { return type; } set { type = value; } }


}