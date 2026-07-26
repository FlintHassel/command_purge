using UnityEngine;

/// <summary>
/// Enum semua state yang mungkin dimiliki player.
/// </summary>
public enum PlayerState
{
    Exploring,
    UsingComputer,
    Cutscene
}

/// <summary>
/// Singleton manager untuk PlayerState.
/// Subscribe ke OnStateChanged untuk bereaksi terhadap perubahan state.
/// Gunakan PlayerStateManager.Instance.SetState(PlayerState.X) untuk mengubah state.
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    // --- Singleton ---
    public static PlayerStateManager Instance { get; private set; }

    // --- Events ---
    /// <summary>Dipanggil setiap kali state berubah. (previousState, newState)</summary>
    public static event System.Action<PlayerState, PlayerState> OnStateChanged;

    // --- State ---
    [Header("State (read-only di Inspector)")]
    [SerializeField] private PlayerState _currentState = PlayerState.Exploring;

    public PlayerState CurrentState => _currentState;

    // --- Unity Lifecycle ---
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad agar persist saat scene Computer di-load additive
        DontDestroyOnLoad(gameObject);
    }

    // --- Public API ---
    /// <summary>
    /// Ubah PlayerState. Tidak akan memicu event jika state sama.
    /// </summary>
    public void SetState(PlayerState newState)
    {
        if (_currentState == newState) return;

        PlayerState prev = _currentState;
        _currentState = newState;

        Debug.Log("[PlayerStateManager] " + prev + " -> " + newState);
        OnStateChanged?.Invoke(prev, newState);
    }

    /// <summary>
    /// Shortcut: cek apakah player sedang menggunakan komputer.
    /// </summary>
    public bool IsUsingComputer => _currentState == PlayerState.UsingComputer;
}
