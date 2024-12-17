using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetArea : MonoBehaviour
{
    public TextMeshProUGUI messageText; // רכיב טקסט להודעה
    public float messageDuration = 2f; // משך זמן להצגת ההודעה

    public bool IsFoodInArea(FoodCarry food)
    {
        // בדיקה אם הקוליידר של האוכל נמצא בתוך הקוליידר של אזור המטרה
        Collider2D areaCollider = GetComponent<Collider2D>();
        Collider2D foodCollider = food.GetComponent<Collider2D>();

        if (areaCollider != null && foodCollider != null)
        {
            return areaCollider.bounds.Intersects(foodCollider.bounds); // חישוב חפיפה
        }
        return false;
    }

    public void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(true); // הצגת הטקסט
            messageText.text = "Good Job!";
            Invoke("HideMessage", messageDuration); // הסתרה לאחר זמן מוגדר
        }
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false); // הסתרת הטקסט
        }
    }
}
