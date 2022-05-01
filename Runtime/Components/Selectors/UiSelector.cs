// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.ObjectPooling : UiSelector.cs
//
// All Rights Reserved

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.Selectors
{
    public class UiSelector : PointerDownAbstractSelector
    {
        [SerializeField] private GraphicRaycaster raycaster;

        protected override GameObject HitObject(Vector2 screenPoint)
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = screenPoint
            };
            var results = new List<RaycastResult>();

            raycaster.Raycast(pointerData, results);
            if (results.Count <= 0)
            {
                return null;
            }

            var res = results.FindIndex(g => mask == (mask | (1 << g.gameObject.layer)));
            return res < 0 ? null : results[res].gameObject;
        }
    }
}
