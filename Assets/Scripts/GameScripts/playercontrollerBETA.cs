// Файл: PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playercontrollerBETA : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [Range(1, 10)] public float turnSpeed = 1f;

    [Header("Rotation Smoothing")]
    [Tooltip("Чем выше, тем плавнее (медленнее) поворот")]
    [SerializeField] private float rotateSmoothness = 10f;

    private Rigidbody rb;
    private Quaternion targetRotation;
    private float _finalTurnSpeed;
    private Enemy_Stats _enemy;
    private Player_stats _player;

    void Start()
    {
        _enemy = new Enemy_Stats();
        _player = new Player_stats();
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        HandleMovement();
        RotateAxisX();
    }

    private void HandleMovement()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(BindingKeysManager.Forward_Key_KEYCODE)) dir += transform.forward;
        if (Input.GetKey(BindingKeysManager.Backward_Key_KEYCODE)) dir -= transform.forward;
        if (Input.GetKey(BindingKeysManager.GoLeft_Key_KEYCODE)) dir -= transform.right;
        if (Input.GetKey(BindingKeysManager.GoRight_Key_KEYCODE)) dir += transform.right;

        Vector3 vel = dir.normalized * moveSpeed;
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }

    private void HandleRotation()
    {
        // получаем желаемый поворот на основе мыши
        float mx = Input.GetAxis("Mouse X") * _finalTurnSpeed;
        targetRotation *= Quaternion.Euler(0f, mx, 0f);  // :contentReference[oaicite:3]{index=3}

        // плавно интерполируем текущий кватернион к целевому
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotateSmoothness  // :contentReference[oaicite:4]{index=4}
        );
    }    private void InversionRotateAxisX()
    {
        _finalTurnSpeed = -turnSpeed;
        HandleRotation();
    }
    private void NotInversionRotateAxisX()
    {
        _finalTurnSpeed = turnSpeed;
        HandleRotation();
    }
    private void RotateAxisX()
    {
        if (PlayerPrefs.GetInt(BooleanSettings.IsInversionX) == 1) InversionRotateAxisX();
        else NotInversionRotateAxisX();
    }

    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.TryGetComponent<EnemyAI>(out var _))
        {
            _player.TakeDamage(_enemy.DamagePerTick);
        }
    }
}

