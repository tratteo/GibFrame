using UnityEditor;

namespace GibFrame.Editor
{
    internal static class ValidatorMenuItems
    {
        [MenuItem("GibFrame/Validators/Validate all groups", false, 0)]
        internal static void ValidateAllGroups() => ValidatorManager.ValidateAllGroupsInAssets();

        [MenuItem("GibFrame/Validators/Validate guardeds", false, 0)]
        internal static void ValidateGuardeds() => ValidatorManager.ValidateGuarded();
    }
}