using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject fimdoJogo;
    public GameObject player;
    public string[] dialogue;
    private int index = 0;

    public AudioClip sound1;
    AudioSource audioSource;

    public float wordSpeed;
    public bool playerIsClose;


    void Start()
    {
        dialogueText.text = "";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }

        if (dialogueText.text == "E ele está dentro da caverna ao lado desta vila!")
        {
            NextStage();
            
        }

        if (dialogueText.text == "Obrigado! Você salvou a nossa vila!")
        {
            FinalStage();
            
        }
    }

    public void NextStage()
    {
        StartCoroutine(WaitTime1());
        
    }

    private void FinalStage()
    {
        StartCoroutine(WaitTime2());
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
            audioSource.PlayOneShot(sound1);
            
        }
    }

    IEnumerator WaitTime1()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Fase 2");
    }
    IEnumerator WaitTime2()
    {
        yield return new WaitForSeconds(1f);
        dialoguePanel.SetActive(false);
        fimdoJogo.SetActive(true);
        player.gameObject.GetComponent<CharacterController2D>().enabled = false;
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }

}
