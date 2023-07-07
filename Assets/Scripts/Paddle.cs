using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public float speed = 70f;
    public float maxBounceAngle = 75f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetPaddle();
    }

    public void ResetPaddle()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = new Vector2(0f, transform.position.y);
    }

    private void Update()
    {
        float xInput;
        xInput = Input.GetAxis("Horizontal");
        direction = new Vector2(xInput, 0);
        
    }

    private void FixedUpdate()
    {
        if (direction != Vector2.zero) {
            rigidbody.AddForce(direction * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Vector2 paddlePosition = transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float maxOffset = collision.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rigidbody.velocity);
            float bounceAngle = (offset / maxOffset) * maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBounceAngle, maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            ball.rigidbody.velocity = rotation * Vector2.up * ball.rigidbody.velocity.magnitude;
        }
    }

}
