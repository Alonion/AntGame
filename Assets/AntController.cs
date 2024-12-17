using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // מהירות התנועה קדימה ואחורה
    public float rotationSpeed = 100f; // מהירות הסיבוב
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // קישור ל-Animator
    }

    void Update()
    {
        // קלט מהמשתמש
        float verticalInput = Input.GetAxis("Vertical"); // קדימה/אחורה
        float horizontalInput = Input.GetAxis("Horizontal"); // ימינה/שמאלה

        // תנועה קדימה ואחורה לפי Local Space
        transform.Translate(Vector3.up * verticalInput * moveSpeed * Time.deltaTime, Space.Self);

        // סיבוב ימינה ושמאלה סביב ציר Z
        transform.Rotate(Vector3.forward, -horizontalInput * rotationSpeed * Time.deltaTime);

        // שליטה על האנימציות
        if (verticalInput != 0)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("Direction", verticalInput); // כיוון התנועה (קדימה או אחורה)
        }
        else
        {
            animator.SetBool("IsWalking", false); // עצירה
        }
    }

    // פונקציה לשינוי מהירות התנועה
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
