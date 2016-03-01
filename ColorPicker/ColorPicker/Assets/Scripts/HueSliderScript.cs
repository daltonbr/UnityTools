using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class HueSliderScript : MonoBehaviour {

    /// <summary>
    ///     Main overlay canvas
    /// </summary>
    public Canvas canvas;

    /// <summary>
    ///     Main color chooser object
    /// </summary>
    public GameObject colorChooser;

    /// <summary>
    ///     Parent huePanel
    /// </summary>
    public Image huePanel;

    /// <summary>
    /// Current hue
    /// </summary>
    public float currentHue { get; set; }

    /// <summary>
    ///     On drag event
    /// </summary>
    public void OnDrag() {
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float minY = huePanel.transform.position.y - huePanel.GetComponent<RectTransform>().rect.height + (huePanel.GetComponent<RectTransform>().rect.height / 2) + 3;
        float maxY = huePanel.transform.position.y + (huePanel.GetComponent<RectTransform>().rect.height / 2);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        float newY = canvas.transform.TransformPoint(pos).y;
        newY = Mathf.Clamp(newY, minY, maxY);
        transform.position = new Vector2(0, newY);

        float range = maxY - minY;
        float diff = newY - minY;
        currentHue = diff / range;

        HSBColor hsbColor = new HSBColor(currentHue, 1.0f, 1.0f);

        ColorChooserScript script = colorChooser.GetComponent<ColorChooserScript>();
        script.SetColor(hsbColor.ToColor(), true);
    }

    /// <summary>
    ///     On drop event
    /// </summary>
    public void OnDrop() {
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
    }

    /// <summary>
    ///     Move the hue slider to the hue of the input color
    /// </summary>
    /// 
    /// <param name="newColor">
    ///     New target color
    /// </param>
    public void MoveToColor(Color newColor) {
        HSBColor hsbColor = new HSBColor(newColor);
        currentHue = hsbColor.h;

        float minY = huePanel.transform.position.y - huePanel.GetComponent<RectTransform>().rect.height + (huePanel.GetComponent<RectTransform>().rect.height / 2) + 3;
        float maxY = huePanel.transform.position.y + (huePanel.GetComponent<RectTransform>().rect.height / 2) - 3;

        float range = 204;
        float newY = (-207) + (range * currentHue);

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newY);
    }

}
// End of class