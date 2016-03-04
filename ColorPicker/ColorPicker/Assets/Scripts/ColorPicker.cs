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
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///     Namespace for scripts relating to the ColorPicker asset
/// </summary>
namespace ColorPicker {

    /// <summary>
    ///     Main color picker script
    /// </summary>
    public class ColorPicker : MonoBehaviour {

        /// <summary>
        ///     Previously selected color (in HSB), default is black
        /// </summary>
        public static HSBColor previousColor = new HSBColor(1.0f, 0.0f, 0.0f);

        /// <summary>
        ///     Array of objects representing recently selected colors
        /// </summary>
        public GameObject[] recentColorImages;

        /// <summary>
        ///     Currently selected color (in HSB)
        /// </summary>
        private HSBColor currentColor;

        /// <summary>
        ///     Currently selected color image/label
        /// </summary>
        private Image currentColorImage;

        /// <summary>
        ///     Previously selected color image/label
        /// </summary>
        private Image previousColorImage;

        /// <summary>
        ///     Input fields for red, green, and values
        /// </summary>
        private InputField redInputField, greenInputField, blueInputField;

        /// <summary>
        ///     Input field for HEX values
        /// </summary>
        private InputField hexInputField;

        /// <summary>
        ///     HueSlider script instance
        /// </summary>
        private HueSlider hueSlider;

        /// <summary>
        ///     HSBSwatch script instance
        /// </summary>
        private HSBSwatch hsbSwatch;

        /// <summary>
        ///     On wake up, find various UI components and reset the HSB shader to match the initially selected color
        /// </summary>
        public void Start() {

            // Find UI components
            currentColorImage = gameObject.transform.Find("currentColor").GetComponent<Image>();
            previousColorImage = gameObject.transform.Find("previousColor").GetComponent<Image>();
            redInputField = gameObject.transform.Find("redControls/redInputField").GetComponent<InputField>();
            greenInputField = gameObject.transform.Find("greenControls/greenInputField").GetComponent<InputField>();
            blueInputField = gameObject.transform.Find("blueControls/blueInputField").GetComponent<InputField>();
            hexInputField = gameObject.transform.Find("hexInputField").GetComponent<InputField>();

            // Find script instances
            hueSlider = gameObject.transform.Find("huePanel/slider").GetComponent<HueSlider>();
            hsbSwatch = gameObject.transform.Find("hsbSwatch/crosshair").GetComponent<HSBSwatch>();

            // Fire an initial event to update the HSB shader
            RGBChanged();

            recentColorImages[0].GetComponent<Image>().color = previousColor.ToColor();
        }

        /// <summary>
        ///     Update the current color based on changes from a different UI component
        /// </summary>
        /// 
        /// <param name="hue">
        ///     New hue value (if less than 0.0, assume hue unchanged)
        /// </param>
        /// <param name="saturation">
        ///     New saturation value (if less than 0.0, assume saturation unchanged)
        /// </param>
        /// <param name="brightness">
        ///     New brightness value (if less than 0.0, assume brightness unchanged)
        /// </param>
        public void SetColor(float hue, float saturation, float brightness) {

            // Updated HSB values
            float newHue = currentColor.h;
            float newSaturation = currentColor.s;
            float newBrightness = currentColor.b;

            // Update the HUE
            if (hue >= 0.0f) newHue = hue;
            // Update the saturation
            if (saturation > 0.0f) newSaturation = saturation;
            // Update the brightness
            if (brightness > 0.0f) newBrightness = brightness;

            currentColor = new HSBColor(newHue, newSaturation, newBrightness);
            Color rgbColor = currentColor.ToColor();

            // Update the UI components with the new color's values
            currentColorImage.color = rgbColor;
            hexInputField.text = "#" + ColorToHex(rgbColor);

            float r = (float) Math.Truncate(rgbColor.r * 1000.0f) / 1000.0f;
            float g = (float) Math.Truncate(rgbColor.g * 1000.0f) / 1000.0f;
            float b = (float) Math.Truncate(rgbColor.b * 1000.0f) / 1000.0f;

            // Update the RGB Input Fields
            redInputField.text = ((int)(r * 255)).ToString();
            greenInputField.text = ((int)(g * 255)).ToString();
            blueInputField.text = ((int)(b * 255)).ToString();

            if (saturation < 0.0f || brightness < 0.0f) {
                // Update the shader on the HSB swatch
                hsbSwatch.MoveToColor(new HSBColor(rgbColor));
            }
        }

        /// <summary>
        ///     Update appropriate UI fields after a new Hex value is input
        /// </summary>
        public void HexChanged() {
            string hex = hexInputField.text;
            if (hex.StartsWith("#")) {
                hex = hex.Replace("#", "");
            }
            if (hex.Length != 6) return;

            // Convert the hex to a color
            Color newColorRGB = HexToColor(hex);
            currentColor = new HSBColor(newColorRGB);

            // Update the current color image
            currentColorImage.color = newColorRGB;

            // Update the RGB Input Fields
            redInputField.text = ((int)(newColorRGB.r * 255)).ToString();
            greenInputField.text = ((int)(newColorRGB.g * 255)).ToString();
            blueInputField.text = ((int)(newColorRGB.b * 255)).ToString();

            // Update the position of the Hue slider
            hueSlider.MoveToColor(newColorRGB);

            // Update the shader on the HSB swatch
            hsbSwatch.MoveToColor(new HSBColor(newColorRGB));
        }

        /// <summary>
        ///     Update appropriate UI fields after any one of the RGB values are changed
        /// </summary>
        public void RGBChanged() {

            // Get the RGB values
            float r = float.Parse(redInputField.text);
            float g = float.Parse(greenInputField.text);
            float b = float.Parse(blueInputField.text);

            // Ensure that colors are within the right range (0 to 255)
            if (r > 255) {
                redInputField.text = "255";
                r = 255;
            }
            if (g > 255) {
                greenInputField.text = "255";
                g = 255;
            }
            if (b > 255) {
                blueInputField.text = "255";
                b = 255;
            }

            // Scale to between 0.0 and 1.0
            r /= 255;
            g /= 255;
            b /= 255;

            // Convert the RGB values to a color
            Color newColorRGB = new Color(r, g, b);
            currentColor = new HSBColor(newColorRGB);

            // Update the current color image
            currentColorImage.color = newColorRGB;

            // Update the hexInputField
            hexInputField.text = "#" + ColorToHex(newColorRGB);

            // Update the position of the Hue slider
            hueSlider.MoveToColor(newColorRGB);

            // Update the shader on the HSB swatch
            hsbSwatch.MoveToColor(new HSBColor(newColorRGB));
        }

        /// <summary>
        ///     Revert to the input image's color
        /// </summary>
        public void RevertToColor(Image image) {
            currentColor = new HSBColor(image.color);
            Color currentColorRGB = currentColor.ToColor();

            // Update the current color image
            currentColorImage.color = currentColorRGB;

            // Update the RGB Input Fields
            redInputField.text = ((int)(currentColorRGB.r * 255)).ToString();
            greenInputField.text = ((int)(currentColorRGB.g * 255)).ToString();
            blueInputField.text = ((int)(currentColorRGB.b * 255)).ToString();

            // Update the hexInputField
            hexInputField.text = "#" + ColorToHex(currentColorRGB);

            // Update the position of the Hue slider
            hueSlider.MoveToColor(currentColorRGB);

            // Update the shader on the HSB swatch
            hsbSwatch.MoveToColor(new HSBColor(currentColorRGB));
        }

        /// <summary>
        ///     Store the current color choosing, remembering the previous color
        /// </summary>
        public void OK() {
            previousColor = currentColor;
            previousColorImage.color = previousColor.ToColor();

            for (int i = (recentColorImages.Length - 1); i >= 0; i--) {
                if(i == 0) {
                    recentColorImages[i].GetComponent<Image>().color = previousColor.ToColor();
                } else {
                    Color beforeColor = recentColorImages[i - 1].GetComponent<Image>().color;
                    recentColorImages[i].GetComponent<Image>().color = beforeColor;
                }
            }

           gameObject.SetActive(false);
        }

        /// <summary>
        ///     Cancel the current color choosing
        /// </summary>
        public void Cancel() {
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Converts the input Hex string into a Color
        /// </summary>
        /// 
        /// <param name="hex">
        ///     Hex string (without preceeding #)
        /// </param>
        /// 
        /// <returns>
        ///     Resulting Color object
        /// </returns>
        private Color HexToColor(string hex) {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }

        /// <summary>
        ///     Returns the input color as a Hex string
        /// </summary>
        /// 
        /// <param name="color">
        ///     Color to convert
        /// </param>
        /// 
        /// <returns>
        ///     Resulting Hex string
        /// </returns>
        private string ColorToHex(Color32 color) {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

    }
    // End of class.

}
// End of namespace.