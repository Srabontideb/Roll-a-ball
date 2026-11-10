using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    // Number of pickups collected, and how many exist in the level.
    private int count;
    private int totalPickUps;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Becomes true once the player has won or lost, so it can't happen twice.
    private bool gameOver = false;

    // Speed at which the player moves.
    public float speed = 10;

    // UI text that shows the count.
    public TextMeshProUGUI countText;

    // UI object that shows "You Win!" or "You Lose!".
    public GameObject winTextObject;

    // NEW: the "Play Again" button, shown when the game ends.
    public GameObject restartButton;

    // NEW: images the ball switches between each time it collects a pickup.
    public Texture[] ballTextures;
    private Renderer ballRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRenderer = GetComponent<Renderer>();

        count = 0;

        // NEW: count the pickups automatically instead of typing 12.
        totalPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;

        winTextObject.SetActive(false);

        if (restartButton != null)
        {
        restartButton.SetActive(false);
        }
        ChangeBallTexture();
        SetCountText();
    }

    // Called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    // Losing: the enemy touched the ball.
    private void OnCollisionEnter(Collision collision)
    {
        if (gameOver)
        {
        return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
        gameOver = true;

        winTextObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = "you lost!";
        winTextObject.GetComponent<TextMeshProUGUI>().color = Color.red;

        ShowRestartButton();

        Destroy(gameObject);
        }
    }

    // Collecting pickups.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
        other.gameObject.SetActive(false);
        count = count + 1;

        ChangeBallTexture();
        SetCountText();
        }
    }

    void SetCountText()
    {
        // NEW: shows progress like "Count: 3 / 12".
        countText.text = "Count: " + count.ToString() + " / " + totalPickUps.ToString();

        if (count >= totalPickUps && !gameOver)
        {
        gameOver = true;

        winTextObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = "You Won!";

        // Remove the whole enemy (parent and body), not just the red box.
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
        Destroy(enemy.transform.root.gameObject);
        }

        ShowRestartButton();
        }
    }

    // NEW: gives the ball the next image from the list.
    void ChangeBallTexture()
    {
        if (ballTextures != null && ballTextures.Length > 0)
        {
        ballRenderer.material.mainTexture = ballTextures[count % ballTextures.Length];
        }
    }

    // NEW: shows the "Play Again" button.
    void ShowRestartButton()
    {
        if (restartButton != null)
        {
        restartButton.SetActive(true);
        }
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
        gm.EndGame();
        }
    }
}