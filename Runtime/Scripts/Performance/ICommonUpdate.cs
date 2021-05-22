//Copyright (c) matteo
//ICommonUpdate.cs - com.tratteo.gibframe

namespace GibFrame.Performance
{
    public interface ICommonUpdate : ICommon
    {
        void CommonUpdate(float deltaTime);
    }
}