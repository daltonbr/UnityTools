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
    ///     This script handles changing the cursor when over an interactable UI component
    /// </summary>
    public class CustomCursor : MonoBehaviour {

        /// <summary>
        ///     Custom cursor to show in attached UI element
        /// </summary>
        public Texture2D newCursor;

        /// <summary>
        ///     Width of newCursor texture in pixels
        /// </summary>
        private float width;

        /// <summary>
        ///     Height of newCursor texture in pixels
        /// </summary>
        private float height;

        /// <summary>
        ///     Should we currently be drawing the custom cursor
        /// </summary>
        private bool isCustom;

        /// <summary>
        ///     On wakeup
        /// </summary>
        public void Awake() {
            width = newCursor.width;
            height = newCursor.height;
        }

        /// <summary>
        ///     On disposal, revert back to the normal cursor
        /// </summary>
        public void OnDisable() {
            CustomCursor[] scripts = gameObject.GetComponentsInChildren<CustomCursor>();
            foreach(CustomCursor script in scripts) {
                script.OnExit();
            }
        }

        /// <summary>
        ///     On mouse entry of UI element
        /// </summary>
        public void OnEnter() {
            Cursor.visible = false;
            isCustom = true;
        }

        /// <summary>
        ///     On mouse exit of UI element
        /// </summary>
        public void OnExit() {
            Cursor.visible = true;
            isCustom = false;
        }

        /// <summary>
        ///     On GUI render, draw the customCursor texture at the mouse position
        /// </summary>
        public void OnGUI() {
            if (!isCustom) return;

            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            Rect rect = new Rect(mouseX - (width / 2) + 6, Screen.height - mouseY, width, height);
            GUI.DrawTexture(rect, newCursor);
        }

    }
    // End of class.

}
// End of namespace.