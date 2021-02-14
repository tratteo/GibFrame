// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.UI : ToggleButton.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.UI
{
    public class ToggleButton : GButton
    {
        public bool colorChange;
        public bool spriteChange;
        public Color enabledColor;
        public Color disabledColor;
        public Sprite enabledSprite;
        public Sprite disabledSprite;

        private Image icon;

        public void SetState(bool state)
        {
            if (state)
            {
                icon.sprite = enabledSprite != null ? enabledSprite : icon.sprite;
                icon.color = enabledColor;
            }
            else
            {
                icon.sprite = disabledSprite != null ? disabledSprite : icon.sprite;
                icon.color = disabledColor;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            icon = GetComponentInChildren<Image>();
        }
    }
}