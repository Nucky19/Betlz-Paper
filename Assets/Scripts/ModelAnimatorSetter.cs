using UnityEngine;

public class ModelAnimatorSetter : MonoBehaviour
{
    private Animator modelAnimator;

    void Awake()
    {
        modelAnimator = GetComponent<Animator>(); 
    }

    void OnEnable()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetAnimator(modelAnimator); 
        }
    }
}
