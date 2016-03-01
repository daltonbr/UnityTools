using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class HSLDraggableScript : MonoBehaviour {

    /// <summary>
    ///     Main overlay canvas
    /// </summary>
    public Canvas canvas;

    /// <summary>
    ///     Main color chooser object
    /// </summary>
    public GameObject colorChooser;

    /// <summary>
    ///     Parent hslPanel
    /// </summary>
    public Image hslPanel;

    /// <summary>
    ///     Hue slider object
    /// </summary>
    public GameObject hueSlider;

    /// <summary>
    ///     On drag event
    /// </summary>
    public void OnDrag() {
        float minX = hslPanel.transform.position.x - (hslPanel.GetComponent<RectTransform>().rect.width / 2);
        float maxX = minX + hslPanel.GetComponent<RectTransform>().rect.width;

        float minY = hslPanel.transform.position.y - hslPanel.GetComponent<RectTransform>().rect.height + (hslPanel.GetComponent<RectTransform>().rect.height / 2) + 3;
        float maxY = hslPanel.transform.position.y + (hslPanel.GetComponent<RectTransform>().rect.height / 2) - 3;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        float newY = canvas.transform.TransformPoint(pos).y;
        newY = Mathf.Clamp(newY, minY, maxY);

        float newX = canvas.transform.TransformPoint(pos).x;
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector2(newX, newY);

        float rangeX = maxY - minY;
        float rangeY = maxY - minY;

        float hue = hueSlider.GetComponent<HueSliderScript>().currentHue;
        float s = (newX - minX) / rangeX;
        float b = (newY - minY) / rangeY;

        HSBColor hsbColor = new HSBColor(hue, s, b);

        ColorChooserScript script = colorChooser.GetComponent<ColorChooserScript>();
        script.SetColor(hsbColor.ToColor(), false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <param name="newColor">
    /// 
    /// </param>
    public void MoveToColor(Color newColor) {
        HSBColor hsbColor = new HSBColor(newColor);

        float minX = hslPanel.transform.position.x - (hslPanel.GetComponent<RectTransform>().rect.width / 2);
        float maxX = minX + hslPanel.GetComponent<RectTransform>().rect.width;

        float minY = hslPanel.transform.position.y - hslPanel.GetComponent<RectTransform>().rect.height + (hslPanel.GetComponent<RectTransform>().rect.height / 2) + 3;
        float maxY = hslPanel.transform.position.y + (hslPanel.GetComponent<RectTransform>().rect.height / 2) - 3;

        float rangeX = maxY - minY;
        float rangeY = maxY - minY;

        float newX = minX + (hsbColor.s * rangeX);
        float newY = minY + (hsbColor.b * rangeY);

    }

}
// End of class