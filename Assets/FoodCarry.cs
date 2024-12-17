using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCarry : MonoBehaviour
{
    private bool isCarried = false; // האם האוכל נישא
    private Transform antTransform; // מיקום הנמלה
    private bool canPickUp = false; // האם ניתן להרים את האוכל
    private float balanceOffset = 0f; // סטיית האיזון
    private float maxBalanceOffset = 1.2f; // מקסימום סטייה לפני נפילה
    private float balanceSpeed = 1.5f; // מהירות שינוי האיזון (לחיצה על מקשי R/E)
    private float rotationSpeed = 60f; // מהירות הרוטציה של האוכל
    private float gravityEffect = 0.2f; // השפעת הגרביטציה המדומה
    private float rotationImpact = 0.03f; // השפעת רוטציית הנמלה על האיזון (מוקטן)
    private float gravityThreshold = 0.1f; // סף הזווית שממנו הגרביטציה מתחילה להשפיע
    private Vector3 initialPosition; // מיקום ההתחלה של האוכל
    private float previousRotation = 0f; // רוטציה קודמת של הנמלה

    private TargetArea targetArea; // משתנה לאזור המטרה

    void Start()
    {
        initialPosition = transform.position; // שמירת מיקום ההתחלה של האוכל
        targetArea = FindObjectOfType<TargetArea>(); // מציאת TargetArea בסצנה
    }

    void Update()
    {
        if (isCarried && antTransform != null)
        {
            // האוכל עוקב אחרי מיקום הנמלה עם ה-offset החדש
            transform.position = antTransform.position + new Vector3(0, 0.3f, 0);

            // השפעת רוטציה של הנמלה על האיזון
            float currentRotation = antTransform.eulerAngles.z; // קבלת הרוטציה הנוכחית
            float rotationChange = Mathf.DeltaAngle(previousRotation, currentRotation); // חישוב השינוי ברוטציה
            balanceOffset += rotationChange * rotationImpact; // הוספת ההשפעה של הרוטציה על האיזון
            previousRotation = currentRotation; // עדכון הרוטציה הקודמת

            // השפעת גרביטציה רק כאשר הסטייה גדולה מסף מסוים
            if (Mathf.Abs(balanceOffset) > gravityThreshold)
            {
                balanceOffset += Mathf.Sign(balanceOffset) * gravityEffect * Time.deltaTime;
            }

            // שליטה על האיזון עם מקשי R ו-E
            if (Input.GetKey(KeyCode.R))
            {
                balanceOffset += balanceSpeed * Time.deltaTime; // איזון ימינה
            }
            if (Input.GetKey(KeyCode.E))
            {
                balanceOffset -= balanceSpeed * Time.deltaTime; // איזון שמאלה
            }

            // רוטציה של האוכל לפי האיזון
            transform.rotation = Quaternion.Euler(0, 0, -balanceOffset * rotationSpeed);

            // בדיקה אם האוכל יוצא מאיזון ונופל
            if (Mathf.Abs(balanceOffset) > maxBalanceOffset)
            {
                Debug.Log("Food dropped due to imbalance!");
                ReturnToStart(); // חזרה לנקודת ההתחלה
            }
        }

        // חיבור או שחרור האוכל בלחיצה על רווח
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isCarried)
            {
                Debug.Log("Food dropped manually!");
                DropFood();
            }
            else if (canPickUp)
            {
                Debug.Log("Food picked up by Ant!");
                PickUpFood();
            }
        }
    }

    void ReturnToStart()
    {
        if (isCarried && antTransform != null) // בדיקה אם האוכל נישא
        {
            // החזרת המהירות המקורית לנמלה
            AntMovement antMovement = antTransform.GetComponent<AntMovement>();
            if (antMovement != null)
            {
                antMovement.SetMoveSpeed(2f); // החזרת מהירות ל-2f
            }
        }

        isCarried = false; // שחרור האוכל
        antTransform = null; // איפוס מיקום הנמלה
        balanceOffset = 0f; // איפוס האיזון
        transform.rotation = Quaternion.identity; // איפוס הרוטציה
        transform.position = initialPosition; // החזרה לנקודת ההתחלה
    }

    void PickUpFood()
    {
        isCarried = true; // חיבור האוכל לנמלה
        antTransform = GameObject.FindWithTag("Ant").transform; // שמירת מיקום הנמלה

        // הורדת מהירות הנמלה
        AntMovement antMovement = antTransform.GetComponent<AntMovement>();
        if (antMovement != null)
        {
            antMovement.SetMoveSpeed(0.5f); // שינוי מהירות ל-0.5f
        }
    }

    void DropFood()
    {
        if (isCarried && antTransform != null) // בדיקה אם האוכל נישא
        {
            // החזרת המהירות המקורית לנמלה
            AntMovement antMovement = antTransform.GetComponent<AntMovement>();
            if (antMovement != null)
            {
                antMovement.SetMoveSpeed(2f); // החזרת מהירות ל-2f
            }
        }

        isCarried = false; // שחרור האוכל
        antTransform = null; // איפוס מיקום הנמלה
        balanceOffset = 0f; // איפוס האיזון
        transform.rotation = Quaternion.identity; // איפוס הרוטציה

        // בדיקה אם האוכל לא נישא והוא נמצא באזור המטרה
        if (targetArea != null && !isCarried && targetArea.IsFoodInArea(this))
        {
            Debug.Log("Food delivered successfully!");
            gameObject.SetActive(false); // העלמת האוכל
            targetArea.ShowMessage(); // הצגת הודעה
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCarried && collision.gameObject.CompareTag("Ant"))
        {
            Debug.Log("Ant is in pickup range!");
            canPickUp = true; // אפשר להרים את האוכל
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ant"))
        {
            Debug.Log("Ant left pickup range!");
            canPickUp = false; // אי אפשר להרים את האוכל
        }
    }
}
