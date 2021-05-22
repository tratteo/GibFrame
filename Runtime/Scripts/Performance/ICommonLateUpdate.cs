//Copyright (c) matteo
//ICommonLateUpdate.cs - com.tratteo.gibframe

namespace GibFrame.Performance
{
    public interface ICommonLateUpdate : ICommon
    {
        void CommonLateUpdate(float deltaTime);
    }
}