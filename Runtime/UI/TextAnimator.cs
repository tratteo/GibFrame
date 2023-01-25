using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.UI
{
    [RequireComponent(typeof(Text))]
    public class TextAnimator : MonoBehaviour
    {
        public enum SplitParadigm
        { WORDS, LETTERS }

        [SerializeField] private float elementsPerSecond = 2F;
        [SerializeField] private SplitParadigm splitParadigm = SplitParadigm.WORDS;
        private Text target;

        protected void OnEnable()
        {
            StartCoroutine(Animate_C());
        }

        private void Awake()
        {
            target = GetComponent<Text>();
        }

        private IEnumerator Animate_C()
        {
            var delay = new WaitForSecondsRealtime(1F / elementsPerSecond);
            var builder = new StringBuilder();
            var finalText = target.text;
            var arr = GetElements();
            target.text = string.Empty;
            for (var i = 0; i < arr.Length; i++)
            {
                yield return delay;
                builder.Append(arr[i]);
                if (splitParadigm == SplitParadigm.WORDS && i < arr.Length - 1)
                {
                    builder.Append(" ");
                }
                target.text = builder.ToString();
            }
            target.text = finalText;
        }

        private string[] GetElements()
        {
            return splitParadigm switch
            {
                SplitParadigm.WORDS => target.text.Split(' '),
                SplitParadigm.LETTERS => target.text.ToCharArray().ToList().ConvertAll(c => c.ToString()).ToArray(),
                _ => target.text.Split(' ')
            };
        }
    }
}