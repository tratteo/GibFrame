using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InspectorGrouper : MonoBehaviour
{
    [SerializeField] private List<Group> groups;

    public List<Group> Groups
    {
        get
        {
            groups ??= new List<Group>();
            return groups;
        }
    }

    public Group GroupOf(Component component) => groups.FirstOrDefault(g => g.Members.Contains(component));

    [System.Serializable]
    public class Group
    {
        public string name;
        public bool isVisible = true;
        public bool isEditable = true;
        [SerializeField] private List<Component> components;

        public List<Component> Members => components;

        public Group()
        {
            components = new List<Component>();
        }
    }
}