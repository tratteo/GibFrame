using GibFrame.Editor.Validators;
using UnityEditor;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(ValidatorGroup))]
    public class ValidatorGroupCustomEditor : ValidatorCustomEditor
    {
        protected override void DrawProperties()
        {
            this.PropertyField("validators", "Validators");
        }
    }
}