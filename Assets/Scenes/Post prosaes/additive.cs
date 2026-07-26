using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk mengelola scene

public class additive : MonoBehaviour
{
    [Tooltip("Ketik nama scene kedua yang ingin digabungkan di sini")]
    public string namaSceneKedua;

    void Start()
    {
        // Memanggil scene kedua menggunakan mode Additive
        LoadSceneTambahan();
    }

    public void LoadSceneTambahan()
    {
        // Pengecekan agar tidak meload scene yang sama berkali-kali
        if (!SceneManager.GetSceneByName(namaSceneKedua).isLoaded)
        {
            SceneManager.LoadScene(namaSceneKedua, LoadSceneMode.Additive);
            Debug.Log("Scene " + namaSceneKedua + " berhasil ditambahkan!");
        }
    }
}