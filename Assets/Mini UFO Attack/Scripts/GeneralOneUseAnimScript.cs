using UnityEngine;

public class GeneralOneUseAnimScript : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo stateInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
