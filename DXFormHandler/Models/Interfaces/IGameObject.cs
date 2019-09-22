using DXFormHandler.Models.Objects;
using SharpDX.Mathematics.Interop;
using System;


namespace DXFormHandler.Models.Interfaces
{
    public interface IGameObject
    {
        ObjectTypeEnum tag {
            get;
            set;
        }
    }
}
