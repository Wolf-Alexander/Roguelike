using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GoalTrigger : MonoBehaviour
{
    [Header("UI Referenz")]
    [Tooltip("Das UI-Objekt 'Finished', das beim Erreichen des Ziels aktiviert wird")]
    public GameObject finishObject;

    [Header("Einstellungen")]
    [Tooltip("Tag, den der Spieler haben muss (Standard: 'Player')")]
    public string playerTag = "Player";

    [Tooltip("Soll die Muenze nach dem Einsammeln verschwinden?")]
    public bool hideCoinOnCollect = true;

    [Tooltip("Soll das Spiel bei Erreichen des Ziels pausiert werden?")]
    public bool pauseGameOnFinish = false;

    private bool alreadyTriggered = false;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        // Komponenten einmal zwischenspeichern statt bei jedem Trigger neu zu suchen
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Haeufigste Fehlerquelle: Collider ist kein Trigger
        if (!col.isTrigger)
        {
            Debug.LogWarning("GoalTrigger: Der Collider2D ist kein Trigger! " +
                              "Bitte 'Is Trigger' im Inspector aktivieren.", this);
        }
    }

    private void Start()
    {
        if (finishObject != null)
        {
            finishObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GoalTrigger: Kein 'Finish Object' zugewiesen!", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"GoalTrigger: Kontakt mit '{other.gameObject.name}' (Tag: '{other.tag}')");

        if (alreadyTriggered) return;

        if (!other.CompareTag(playerTag))
        {
            Debug.LogWarning($"GoalTrigger: '{other.gameObject.name}' hat nicht den Tag '{playerTag}'. " +
                              "Tag beim Spieler-Objekt pruefen!", this);
            return;
        }

        alreadyTriggered = true;
        ShowFinishObject();

        if (hideCoinOnCollect)
        {
            if (sr != null) sr.enabled = false;
            if (col != null) col.enabled = false;
        }

        if (pauseGameOnFinish)
        {
            Time.timeScale = 0f;
        }
    }

    private void ShowFinishObject()
    {
        if (finishObject != null)
        {
            finishObject.SetActive(true);
        }
    }
}