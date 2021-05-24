// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SceneManagement : AsyncLoadIndexSaver.cs
//
// All Rights Reserved

namespace GibFrame.SceneManagement
{
    internal static class AsyncLoadIndexSaver
    {
        private static int indexToPreload;

        internal static int GetSceneIndexToPreload()
        {
            return indexToPreload;
        }

        internal static void SetIndexToPreload(int index)
        {
            indexToPreload = index;
        }
    }
}
