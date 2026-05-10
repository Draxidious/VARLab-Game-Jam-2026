using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI player1PointsText;
    [SerializeField] private TextMeshProUGUI player2PointsText;
    [SerializeField] private TextMeshProUGUI player1SetsText;
    [SerializeField] private TextMeshProUGUI player2SetsText;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Buttons")]
    [SerializeField] private Button resetButton;

    // Global score state
    public static int Player1Sets { get; private set; }
    public static int Player2Sets { get; private set; }
    public static int Player1Points { get; private set; }
    public static int Player2Points { get; private set; }

    private static readonly string[] PointLabels = { "0", "15", "30", "40" };
    private bool inDeuce;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetAll);

        ResetAll();
    }

    // ?? Public scoring methods called by Player1Controller / Player2Controller
    public void Player1Scores()
    {
        AwardPoint(1);
    }

    public void Player2Scores()
    {
        AwardPoint(2);
    }

    // Point logic
    private void AwardPoint(int scorer)
    {
        if (scorer == 1) Player1Points++;
        else Player2Points++;

        // Check deuce (both at 40 / 3 points each)
        if (!inDeuce && Player1Points == 3 && Player2Points == 3)
        {
            inDeuce = true;
            UpdateUI();
            return;
        }

        if (inDeuce)
        {
            int diff = Player1Points - Player2Points;
            if (Mathf.Abs(diff) >= 2)
                AwardGame(diff > 0 ? 1 : 2);
            else
                UpdateUI();
            return;
        }

        // Normal point win (reached 4 = won game, opponent had less than 3)
        if (Player1Points > 3 || Player2Points > 3)
        {
            AwardGame(scorer);
            return;
        }

        UpdateUI();
    }

    private void AwardGame(int scorer)
    {
        if (scorer == 1) Player1Sets++;
        else Player2Sets++;

        Player1Points = 0;
        Player2Points = 0;
        inDeuce = false;

        UpdateUI();

        if (Player1Sets >= 2 || Player2Sets >= 2)
        {
            statusText.text = $"Player {scorer} Wins the Match!";
            Debug.Log($"[GameManager] Player {scorer} wins the match!");
        }
    }

    // Reset
    public void ResetAll()
    {
        Player1Sets = 0;
        Player2Sets = 0;
        Player1Points = 0;
        Player2Points = 0;
        inDeuce = false;
        UpdateUI();
    }

    // UI
    private void UpdateUI()
    {
        if (player1PointsText != null) player1PointsText.text = GetPointLabel(Player1Points, Player2Points);
        if (player2PointsText != null) player2PointsText.text = GetPointLabel(Player2Points, Player1Points);
        if (player1SetsText != null) player1SetsText.text = Player1Sets.ToString();
        if (player2SetsText != null) player2SetsText.text = Player2Sets.ToString();
        if (statusText != null && Player1Sets < 2 && Player2Sets < 2)
            statusText.text = GetStatusLabel();
    }

    private string GetPointLabel(int mine, int theirs)
    {
        if (!inDeuce) return PointLabels[Mathf.Clamp(mine, 0, 3)];
        if (mine == theirs) return "Deuce";
        return mine > theirs ? "Ad" : "-";
    }

    private string GetStatusLabel()
    {
        if (inDeuce)
        {
            if (Player1Points == Player2Points) return "Deuce";
            return Player1Points > Player2Points ? "Advantage Player 1" : "Advantage Player 2";
        }
        return string.Empty;
    }
}