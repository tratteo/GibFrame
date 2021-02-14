// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.UI : TabGroup.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using GibFrame.Utils;
using UnityEngine;

namespace GibFrame.UI
{
    public class TabGroup : MonoBehaviour
    {
        private List<TabButton> tabButtons;
        private TabButton selected;
        private List<GameObject> views;
        private Action<GameObject, bool> ToggleDelegate = null;

        private TabButton FirstButton { get => Array.Find(tabButtons.ToArray(), (b) => b.transform.GetSiblingIndex() == 0); }

        public void Subscribe(TabButton button)
        {
            tabButtons = tabButtons ?? new List<TabButton>();
            tabButtons.Add(button);
        }

        public void AddView(GameObject view)
        {
            views = views ?? new List<GameObject>();
            views.Add(view);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetButtons();
            if (button != selected)
            {
                button.SetState(TabButton.TabState.HOVER);
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetButtons();
        }

        public void OnTabSelected(TabButton button)
        {
            selected = button;
            ResetButtons();
            button.SetState(TabButton.TabState.SELECTED);
            ToggleViews(button.transform.GetSiblingIndex());
        }

        public void SelectFirst()
        {
            if (tabButtons == null) return;
            ResetButtons();
            ToggleViews();
            OnTabSelected(FirstButton);
        }

        public void OverrideToggleDelegate(Action<GameObject, bool> action)
        {
            ToggleDelegate = action;
        }

        private void Start()
        {
            Transform temp = UnityUtils.GetFirstComponentInChildrenWithName<Transform>(transform.parent.gameObject, "Views", true);
            if (temp == null)
            {
                throw new System.Exception("Unable to fin the views parent");
            }
            views = new List<GameObject>();
            foreach (Transform t in temp)
            {
                views.Add(t.gameObject);
            }
        }

        private void ResetButtons()
        {
            foreach (TabButton button in tabButtons)
            {
                if (button != selected)
                {
                    button.SetState(TabButton.TabState.IDLE);
                }
            }
        }

        private void ToggleViews(int index = -1)
        {
            for (int i = 0; i < views.Count; i++)
            {
                if (ToggleDelegate == null)
                {
                    views[i].SetActive(i == index);
                }
                else
                {
                    ToggleDelegate(views[i], i == index);
                }
            }
        }
    }
}