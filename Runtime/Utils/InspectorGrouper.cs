using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InspectorGrouper : MonoBehaviour
{
    private List<Group> groups;

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
        [SerializeField] private List<Component> components;

        public List<Component> Members => components;

        public bool IsVisible { get; set; } = true;

        public bool IsEditable { get; set; } = true;

        public Group()
        {
            components = new List<Component>();
        }
    }
}