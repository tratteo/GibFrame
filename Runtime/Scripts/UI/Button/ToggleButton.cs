// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.UI : ToggleButton.cs
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
                if (spriteChange)
                {
                    icon.sprite = enabledSprite != null ? enabledSprite : icon.sprite;
                }
                if (colorChange)
                {
                    icon.color = enabledColor;
                }
            }
            else
            {
                if (spriteChange)
                {
                    icon.sprite = disabledSprite != null ? disabledSprite : icon.sprite;
                }
                if (colorChange)
                {
                    icon.color = disabledColor;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            icon = GetComponentInChildren<Image>();
        }
    }
}
