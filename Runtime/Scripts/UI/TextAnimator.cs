using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.UI
{
    [RequireComponent(typeof(Text))]
    public class TextAnimator : MonoBehaviour
    {
        public enum SplitParadigm { WORDS, LETTERS }

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
            WaitForSeconds delay = new WaitForSeconds(1F / elementsPerSecond);
            StringBuilder builder = new StringBuilder();
            string finalText = target.text;
            string[] arr = GetElements();
            target.text = string.Empty;
            for (int i = 0; i < arr.Length; i++)
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
                SplitParadigm.LETTERS => target.text.ToCharArray().ConvertAll(c => c.ToString()),
                _ => target.text.Split(' ')
            };
        }
    }
}
