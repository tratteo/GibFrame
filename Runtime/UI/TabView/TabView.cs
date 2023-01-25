using GibFrame.Extensions;
using UnityEngine;

namespace GibFrame.UI
{
    public class TabView : MonoBehaviour
    {
        [SerializeField] private TabButton tabButtonPrefab;
        private TabGroup tabGroup;
        private Transform viewsParent;

        public GameObject AddContent(GameObject contentPrefab, Sprite icon = null)
        {
            GameObject obj = Instantiate(tabButtonPrefab.gameObject);
            TabButton tab = obj.GetComponent<TabButton>();
            tab.Group = tabGroup;
            tab.Icon = icon;
            tabGroup.Subscribe(tab);
            obj.transform.SetParent(tabGroup.transform, false);

            obj = Instantiate(contentPrefab);
            obj.transform.SetParent(viewsParent, false);
            obj.SetActive(false);
            tabGroup.AddView(obj);
            obj.transform.localPosition = Vector3.zero;

            return obj;
        }

        public void InitializeSelection()
        {
            tabGroup.SelectFirst();
        }

        private void Awake()
        {
            viewsParent = transform.GetFirstComponentInChildrenWithName<Transform>("Views", true);
            tabGroup = GetComponentInChildren<TabGroup>();
        }
    }
}