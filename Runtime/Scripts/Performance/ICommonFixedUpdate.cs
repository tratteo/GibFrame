//Copyright (c) matteo
//ICommonFixedUpdate.cs - com.tratteo.gibframe

namespace GibFrame.Performance
{
    public interface ICommonFixedUpdate : ICommon
    {
        void CommonFixedUpdate(float fixedDeltaTime);
    }
}