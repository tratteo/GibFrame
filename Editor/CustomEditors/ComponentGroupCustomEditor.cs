using UnityEditor;
using UnityEngine;
using static InspectorGrouper;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(InspectorGrouper))]
    internal class ComponentGroupCustomEditor : UnityEditor.Editor
    {
        private const int ButtonWidth = 50;
        private static readonly Color visibleColor = new Color(110 / 255F, 160 / 255F, 130 / 255F);
        private static readonly Color hiddenColor = new Color(190 / 255F, 140 / 255F, 140 / 255F);
        private InspectorGrouper grouper;
        private GUIContent hiddenIcon;
        private GUIContent visibleIcon;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var initialColor = GUI.color;
            var initialEnabled = GUI.enabled;

            if (GUILayout.Button("Add Group"))
            {
                grouper.Groups.Add(new Group());
            }
            if (grouper.Groups is null || grouper.Groups.Count <= 0) return;
            GUILayout.Space(10);
            for (var i = 0; i < grouper.Groups.Count; i++)
            {
                DrawGroup(i, initialEnabled, initialColor);
                if (i < grouper.Groups.Count - 1) GUILayout.Space(10);
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            if (!target)
            {
                foreach (var group in grouper.Groups)
                {
                    ChangeVisibility(group, true);
                }
            }
        }

        private void DrawGroup(int index, bool guiEnabled, Color guiColor)
        {
            var group = grouper.Groups[index];
            if (group.isEditable)
            {
                var objectComponents = grouper.GetComponents<Component>();

                GUILayout.BeginHorizontal();

                group.name = GUILayout.TextField(group.name);

                if (GUILayout.Button("Done", GUILayout.Width(ButtonWidth)))
                    group.isEditable = false;

                GUILayout.EndHorizontal();

                foreach (var comp in objectComponents)
                {
                    var name = comp.GetType().Name;

                    var isInCurrentGroup = group.Members.Contains(comp);
                    var compGroup = grouper.GroupOf(comp);

                    GUI.enabled = comp != grouper && (compGroup == group || compGroup is null);

                    if (compGroup is not null) name += $" [{compGroup.name}]";

                    if (GUILayout.Toggle(isInCurrentGroup, name) != isInCurrentGroup)
                    {
                        if (isInCurrentGroup)
                        {
                            group.Members.Remove(comp);
                            comp.hideFlags &= ~HideFlags.HideInInspector;
                            EditorUtility.SetDirty(target);
                        }
                        else
                        {
                            if (!group.isVisible)
                            {
                                comp.hideFlags |= HideFlags.HideInInspector;
                                EditorUtility.SetDirty(target);
                            }
                            else
                            {
                                comp.hideFlags &= ~HideFlags.HideInInspector;
                                EditorUtility.SetDirty(target);
                            }
                            group.Members.Add(comp);
                        }
                    }
                }

                GUI.enabled = guiEnabled;
            }
            else
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(group.isVisible ? visibleIcon : hiddenIcon, GUILayout.Width(ButtonWidth / 2));

                GUI.color = group.isVisible ? visibleColor : hiddenColor;
                if (GUILayout.Button(group.name))
                    ChangeVisibility(group, !group.isVisible);
                GUI.color = guiColor;
                if (GUILayout.Button("H", GUILayout.Width(ButtonWidth / 2)))
                    ChangeVisibility(group, false);

                if (GUILayout.Button("S", GUILayout.Width(ButtonWidth / 2)))
                    ChangeVisibility(group, true);

                if (GUILayout.Button("Edit", GUILayout.Width(ButtonWidth)))
                {
                    //ChangeVisibility(group, true);
                    group.isEditable = true;
                }

                GUI.enabled = index > 0;
                if (GUILayout.Button("↑", GUILayout.Width(ButtonWidth / 2)))
                {
                    grouper.Groups.RemoveAt(index);
                    grouper.Groups.Insert(index - 1, group);
                    EditorUtility.SetDirty(target);
                }

                GUI.enabled = index < grouper.Groups.Count - 1;
                if (GUILayout.Button("↓", GUILayout.Width(ButtonWidth / 2)))
                {
                    grouper.Groups.RemoveAt(index);
                    grouper.Groups.Insert(index + 1, group);
                    EditorUtility.SetDirty(target);
                }

                GUI.enabled = true;
                if (GUILayout.Button("-", GUILayout.Width(ButtonWidth / 2)))
                {
                    grouper.Groups.Remove(group);
                    ChangeVisibility(group, true);
                    EditorUtility.SetDirty(target);
                }

                GUILayout.EndHorizontal();
                GUI.color = guiColor;
            }
        }

        private void OnEnable()
        {
            grouper = (InspectorGrouper)target;

            visibleIcon = new GUIContent(EditorGUIUtility.IconContent("d_scenevis_visible_hover"));
            hiddenIcon = new GUIContent(EditorGUIUtility.IconContent("d_scenevis_hidden_hover"));

            foreach (var group in grouper.Groups)
            {
                for (var i = 0; i < group.Members.Count; i++)
                {
                    if (!group.Members[i])
                    {
                        group.Members.RemoveAt(i);
                    }
                    else
                    {
                        ChangeVisibility(group, group.isVisible);
                    }
                }
            }
        }

        private void ChangeVisibility(Group group, bool visible)
        {
            for (var i = 0; i < group.Members.Count; i++)
            {
                var current = group.Members[i];
                if (!current)
                {
                    group.Members.RemoveAt(i);
                    continue;
                }

                if (visible)
                {
                    current.hideFlags &= ~HideFlags.HideInInspector;
                    // required if the object was deselected in between
                    CreateEditor(current);
                }
                else
                {
                    current.hideFlags |= HideFlags.HideInInspector;
                }
            }

            group.isVisible = visible;
            if (target)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}