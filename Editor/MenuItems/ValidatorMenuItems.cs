using UnityEditor;

namespace GibFrame.Editor
{
    internal static class ValidatorMenuItems
    {
        [MenuItem("GibFrame/Validators/Validate groups", false, 4)]
        internal static void ValidateAllGroups() => ValidatorManager.ValidateAllGroups();

        [MenuItem("GibFrame/Validators/Validate guardeds", false, 8)]
        internal static void ValidateGuardeds() => ValidatorManager.ValidateGuarded();

        [MenuItem("GibFrame/Validators/Validate", false, 0)]
        internal static void Validate()
        {
            ValidatorManager.ValidateAllGroups();
            ValidatorManager.ValidateGuarded();
        }
    }
}