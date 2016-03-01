
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ColorChooserScript : MonoBehaviour {

    /// <summary>
    ///     Currently selected color image
    /// </summary>
    public Image currentColorImage;

    /// <summary>
    ///     Previously selected color image
    /// </summary>
    public Image previousColorImage;

    /// <summary>
    ///     Previously selected color
    /// </summary>
    public Color previousColor { get; set; }
    
    /// <summary>
    ///     Large HSL Space grid
    /// </summary>
    public Image hslSpace;

    /// <summary>
    ///     HUE slider object
    /// </summary>
    public GameObject hueSlider;

    /// <summary>
    ///     Input field for HEX values
    /// </summary>
    public InputField hexInputField;

    /// <summary>
    ///     Input field for red values
    /// </summary>
    public InputField redInputField;

    /// <summary>
    ///     Input field for green values
    /// </summary>
    public InputField greenInputField;

    /// <summary>
    ///     Input field for blue values
    /// </summary>
    public InputField blueInputField;

    /// <summary>
    ///     For the shader to go back to it's initial state
    /// </summary>
    public void Awake() {
        RGBChanged();
        previousColor = new Color(0.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <param name="color">
    /// 
    /// </param>
    public void SetColor(Color color, bool updateShader) {
        string hex = this.ColorToHex(color);
        hexInputField.text = "#" + hex;

        // Convert the hex to a color
        Color newColor = HexToColor(hex);

        // Update the current color image
        currentColorImage.color = newColor;

        // Update the RGB Input Fields
        redInputField.text = ((int)(newColor.r * 255)).ToString();
        greenInputField.text = ((int)(newColor.g * 255)).ToString();
        blueInputField.text = ((int)(newColor.b * 255)).ToString();

        // Update the HSL space image & Move the hue slider
        if (updateShader) {
            hueSlider.GetComponent<HueSliderScript>().MoveToColor(newColor);
            hslSpace.GetComponent<Image>().material.SetColor("_Color", newColor);
        }
    }

    /// <summary>
    ///     Update appropriate UI fields after a new Hex value is input
    /// </summary>
    public void HexChanged() {
        string hex = hexInputField.text;
        if(hex.StartsWith("#")) {
            hex = hex.Replace("#", "");
        }
        if (hex.Length != 6) return;

        // Convert the hex to a color
        Color newColor = HexToColor(hex);

        // Update the current color image
        currentColorImage.color = newColor;

        // Update the RGB Input Fields
        redInputField.text   = ((int) (newColor.r * 255)).ToString();
        greenInputField.text = ((int) (newColor.g * 255)).ToString();
        blueInputField.text  = ((int) (newColor.b * 255)).ToString();

        // Update the HSL space image
        hslSpace.GetComponent<Image>().material.SetColor("_Color", newColor);

        // Move the hue slider
        hueSlider.GetComponent<HueSliderScript>().MoveToColor(newColor);
    }

    /// <summary>
    ///     Cancel the current color choosing
    /// </summary>
    public void Cancel() {
        gameObject.SetActive(false);
    }

    /// <summary>
    ///     Accept the current color choosing, remembering the previous color
    /// </summary>
    public void OK() {
        previousColor = currentColorImage.color;
        gameObject.SetActive(false);
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
        Color newColor = new Color(r, g, b);

        // Update the current color image
        currentColorImage.color = newColor;

        // Update the hexInputField
        hexInputField.text = "#" + ColorToHex(newColor);

        // Update the HSL space image
        hslSpace.GetComponent<Image>().material.SetColor("_Color", newColor);

        // Move the hue slider
        hueSlider.GetComponent<HueSliderScript>().MoveToColor(newColor);
    }

    /// <summary>
    ///     Revert to the previous color
    /// </summary>
    public void RevertToPrevious() {
        // Convert the RGB values to a color
        Color newColor = previousColor;

        // Update the current color image
        currentColorImage.color = newColor;

        // Update the RGB Input Fields
        redInputField.text = ((int)(newColor.r * 255)).ToString();
        greenInputField.text = ((int)(newColor.g * 255)).ToString();
        blueInputField.text = ((int)(newColor.b * 255)).ToString();

        // Update the hexInputField
        hexInputField.text = "#" + ColorToHex(newColor);

        // Update the HSL space image
        hslSpace.GetComponent<Image>().material.SetColor("_Color", newColor);

        // Move the hue slider
        hueSlider.GetComponent<HueSliderScript>().MoveToColor(newColor);
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