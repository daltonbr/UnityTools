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
    ///     
    /// </summary>
    public class HSBSwatch : MonoBehaviour {

        /// <summary>
        ///     Main overlay canvas
        /// </summary>
        public Canvas canvas;

        /// <summary>
        ///     Main color chooser object
        /// </summary>
        public GameObject colorChooser;

        /// <summary>
        ///     Hue slider object
        /// </summary>
        private HueSlider hueSlider;

        /// <summary>
        ///     Parent hslPanel
        /// </summary>
        private Image hsbSwatch;

        /// <summary>
        ///     On wake up
        /// </summary>
        public void Start() {
            // Find script instances
            hueSlider = gameObject.transform.parent.parent.Find("huePanel/slider").GetComponent<HueSlider>();
            hsbSwatch = gameObject.transform.parent.GetComponent<Image>();
        }

        /// <summary>
        ///     On drag event
        /// </summary>
        public void OnDrag() {
            float minX = hsbSwatch.transform.position.x - (hsbSwatch.GetComponent<RectTransform>().rect.width / 2);
            float maxX = minX + hsbSwatch.GetComponent<RectTransform>().rect.width;

            float minY = hsbSwatch.transform.position.y - hsbSwatch.GetComponent<RectTransform>().rect.height + (hsbSwatch.GetComponent<RectTransform>().rect.height / 2);
            float maxY = hsbSwatch.transform.position.y + (hsbSwatch.GetComponent<RectTransform>().rect.height / 2);

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

            float newY = canvas.transform.TransformPoint(pos).y;
            newY = Mathf.Clamp(newY, minY, maxY);

            float newX = canvas.transform.TransformPoint(pos).x;
            newX = Mathf.Clamp(newX, minX, maxX);

            transform.position = new Vector2(newX, newY);

            float rangeX = maxY - minY;
            float rangeY = maxY - minY;

            float s = (newX - minX) / rangeX;
            float b = (newY - minY) / rangeY;

            // Update the main color picker
            colorChooser.GetComponent<ColorPicker>().SetColor(-1.0f, s, b);
        }

        /// <summary>
        ///     Move the cross hair to the new saturation and brightness
        /// </summary>
        /// 
        /// <param name="hsbColor">
        ///     New color in HSB
        /// </param>
        public void MoveToColor(HSBColor hsbColor) {
            if (hsbSwatch == null) Start();

            float minX = hsbSwatch.transform.position.x - (hsbSwatch.GetComponent<RectTransform>().rect.width / 2);
            float maxX = minX + hsbSwatch.GetComponent<RectTransform>().rect.width;

            float minY = hsbSwatch.transform.position.y - hsbSwatch.GetComponent<RectTransform>().rect.height + (hsbSwatch.GetComponent<RectTransform>().rect.height / 2);
            float maxY = hsbSwatch.transform.position.y + (hsbSwatch.GetComponent<RectTransform>().rect.height / 2);

            float rangeX = maxY - minY;
            float rangeY = maxY - minY;

            float newX = minX + (hsbColor.s * rangeX);
            float newY = minY + (hsbColor.b * rangeY);

            HSBColor huedColor = new HSBColor(hsbColor.h, 1.0f, 1.0f);
            hsbSwatch.GetComponent<Image>().material.SetColor("_Color", huedColor.ToColor());

            transform.position = new Vector2(newX, newY);
        }

    }
    // End of class.

}
// End of namespace.