using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageFeedbackController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject damageTextGO;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private PlayerController player; // Prefab for the damage text
    [SerializeField] public float xOffset; // X offset for the damage text position
    [SerializeField] public float yOffset; // Y offset for the damage text position
    public bool isDamageTextActive = false; // Flag to check if the damage text is active
    private bool isCorroutineActive = false; // Reference to the TextMeshPro component for displaying damage text
    private bool isPlayer = false; // Reference to the TextMeshPro component for displaying damage text

    private float displayDuration = 1f; // Duration to display the damage text
    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        damageTextGO = Resources.Load<GameObject>("DamageText"); // Load the TMP_Text prefab from Resources folder
        player = FindObjectOfType<PlayerController>(); // Find the PlayerController in the scene
    }
    // Start is called before the first frame update
    void Start()
    {
        isPlayer = !gameObject.name.Contains("(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (damageText != null)
        {


            if (isDamageTextActive)
            { // Check if the damage text is active and the coroutine is not running{
                isCorroutineActive = true;
                var gmPos = isPlayer ? player.transform.position : transform.position;
                damageTextGO.transform.position = new Vector3(
                gmPos.x + xOffset,
                gmPos.y + yOffset,
                gmPos.z);

                if (isCorroutineActive)
                {
                    damageTextGO.transform.position += new Vector3(0, 0.005f, 0); // Move the damage text upwards
                    damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - 0.01f);
                }
            }
        }


    }

    public void ShowDamageFeedback(float damageAmount)
    {
        // Corrigir!
        // Preciso que a cada vez que tiver um dano no inimigo, instancie um novo prefab de dano
        displayDuration = 1.5f;
        var instance = Instantiate(damageTextGO, canvas.transform);
        Destroy(damageTextGO,2f);
        var instanceText = instance.GetComponent<TMP_Text>(); // Get the TextMeshPro component from the instantiated prefab
        instance.SetActive(true); // Activate the damage text
        if (isPlayer) instanceText.color = Color.red;
        instanceText.text = $"-{damageAmount}"; // Set the damage amount text

    }
}
