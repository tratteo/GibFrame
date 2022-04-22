using TMPro;
using UnityEngine;

namespace GibFrame
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionInfoText : MonoBehaviour
    {
        [SerializeField] private string prefix = string.Empty;
        private TMP_Text text;

        private void Start()
        {
            text = GetComponent<TMP_Text>();
            text.text = prefix + Application.version;
        }
    }
}
