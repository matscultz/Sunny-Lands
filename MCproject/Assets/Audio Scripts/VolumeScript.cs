using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    public static VolumeScript Instance;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Applica i settaggi di volume all'inizio
        ApplySavedVolume();

        if (SFXSlider != null && musicSlider != null)
        {
            // Collega gli slider ai metodi di regolazione del volume
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        }
    }

    private void Update()
    {
        // Cerca gli slider se non sono stati trovati inizialmente (utile quando cambi scena)
        if (SFXSlider == null && musicSlider == null)
        {
            SFXSlider = GameObject.Find("SFXVolumeSlider")?.GetComponent<Slider>();
            musicSlider = GameObject.Find("MusicVolumeSlider")?.GetComponent<Slider>();

            if (SFXSlider != null && musicSlider != null)
            {
                // Collega di nuovo gli slider agli eventi
                musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
                SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });

                // Imposta gli slider con i valori salvati
                musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1.0f);
                SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            }
        }
    }

    // Funzione per applicare il volume dai PlayerPrefs
    private void ApplySavedVolume()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            // Carica e applica il volume salvato
            float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume");
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume");

            myMixer.SetFloat("MusicVolume", Mathf.Log10(savedMusicVolume) * 20);
            myMixer.SetFloat("SFXVolume", Mathf.Log10(savedSFXVolume) * 20);
        }
        else
        {
            // Se non ci sono preferenze salvate, imposta il volume di default
            myMixer.SetFloat("MusicVolume", Mathf.Log10(1.0f) * 20);
            myMixer.SetFloat("SFXVolume", Mathf.Log10(1.0f) * 20);
        }
    }

    // Imposta il volume della musica
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    // Imposta il volume degli SFX
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}
