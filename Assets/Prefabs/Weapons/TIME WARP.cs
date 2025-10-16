using UnityEngine;

public class GlobalSlowMotion : MonoBehaviour
{
    [Range(0.001f, 1f)]
    public float slowMotionScale = 0.02f; // 1/50 скорости

    private float normalFixedDeltaTime;
    private Animator[] animators;

    void Start()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime;
        animators = FindObjectsOfType<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ApplySlowMo();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            RemoveSlowMo();
        }
    }


    void ApplySlowMo()
    {
        // Время
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = slowMotionScale * Time.timeScale;


        // Анимации
        foreach (var anim in animators)
            anim.speed = slowMotionScale;

        // Звук
        //AudioListener.pitch = slowMotionScale;
    }

    void RemoveSlowMo()
    {
        // Время
        Time.timeScale = 1f;
        Time.fixedDeltaTime = normalFixedDeltaTime;

        // Анимации
        foreach (var anim in animators)
            anim.speed = 1f;

        // Звук
        //AudioListener.pitch = 1f;
    }
}
