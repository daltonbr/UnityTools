/*
*   The MIT License (MIT) - Copyright (c) 2016 M Hillman
* 
*   Permission is hereby granted, free of charge, to any person obtaining a copy
*   of this software and associated documentation files (the "Software"), to deal
*   in the Software without restriction, including without limitation the rights
*   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*   copies of the Software, and to permit persons to whom the Software is
*   furnished to do so, subject to the following conditions:
* 
*   The above copyright notice and this permission notice shall be included in all
*   copies or substantial portions of the Software.
* 
*   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*   AUTHOR/COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
*   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF 
*   OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Namespace for scripts relating to the ColorPicker asset
/// </summary>
namespace ColorPicker {

    /// <summary>
    ///     This script handles changing the currently selected color when the user drags the hueSlider component along the huePanel
    /// </summary>
    public class HueSlider : MonoBehaviour {

        /// <summary>
        ///     Main overlay canvas, used to get world positions of UI components
        /// </summary>
        public Canvas canvas;

        /// <summary>
        ///     Main color chooser object
        /// </summary>
        public GameObject colorChooser;

        /// <summary>
        ///     Parent huePanel
        /// </summary>
        private Image huePanel;

        /// <summary>
        ///     HSB swatch image
        /// </summary>
        private Image hsbSwatch;

        /// <summary>
        ///     On wake up, get the parent UI object
        /// </summary>
        public void Start() {
            huePanel = gameObject.transform.parent.GetComponent<Image>();
            hsbSwatch = gameObject.transform.parent.parent.Find("hsbSwatch").GetComponent<Image>();
        }

        /// <summary>
        ///     On drag event, update the current hue
        /// </summary>
        public void OnDrag() {
            
            // Clamp the Y value so it remains in the parent huePanel
            float minY = huePanel.transform.position.y - huePanel.GetComponent<RectTransform>().rect.height + (huePanel.GetComponent<RectTransform>().rect.height / 2) + 3;
            float maxY = huePanel.transform.position.y + (huePanel.GetComponent<RectTransform>().rect.height / 2);

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

            float newY = canvas.transform.TransformPoint(pos).y;
            newY = Mathf.Clamp(newY, minY, maxY);
            transform.position = new Vector2(transform.position.x, newY);

            // Work out the new hue based on the Y position of the slider
            float range = maxY - minY;
            float diff = newY - minY;
            float hue = diff / range;

            // Push the new hue to the ColorPicker script on the 
            colorChooser.GetComponent<ColorPicker>().SetColor(hue, -1.0f, -1.0f);

            // Push the new color to the HSBSwatch?
            HSBColor huedColor = new HSBColor(hue, 1.0f, 1.0f);
            hsbSwatch.material.SetColor("_Color", huedColor.ToColor());
        }


        /// <summary>
        ///     Move the hue slider to the hue of the input color
        /// </summary>
        /// 
        /// <param name="newColor">
        ///     New target color
        /// </param>
        public void MoveToColor(Color newColor) {
            if (huePanel == null) Start();
            HSBColor hsbColor = new HSBColor(newColor);

            float minY = huePanel.transform.position.y - huePanel.GetComponent<RectTransform>().rect.height + (huePanel.GetComponent<RectTransform>().rect.height / 2) + 3;
            float maxY = huePanel.transform.position.y + (huePanel.GetComponent<RectTransform>().rect.height / 2) - 3;

            float range = 204;
            float newY = (-207) + (range * hsbColor.h);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newY);
        }

    }
    // End of class.

}
// End of namespace.