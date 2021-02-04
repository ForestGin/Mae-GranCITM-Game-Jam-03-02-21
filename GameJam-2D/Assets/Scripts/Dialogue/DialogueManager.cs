﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private readonly List<char> punctuationCharacters = new List<char>
    {
        '.',
        ',',
        '!',
        '?'
    };

    public static DialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Fix this" + gameObject.name);
        }
        else
        {
            instance = this;
        }
    }

    public bool inDialogue = false;
    public GameObject dialogueBox;

    public GameObject dialogueName;
    public GameObject dialogueText;
    public Image dialoguePortrait;
    public float delay = 0.001f;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();

    private bool isCurrentlyTyping;
    private string completeText;

    private void Start()
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
    }

    public void EnqueueDialogue(DialogueBase db)
    {
        if (inDialogue) return;
        inDialogue = true;

        dialogueBox.SetActive(true);
        dialogueInfo.Clear();



        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();

    }

    public void DequeueDialogue()
    {
        if (isCurrentlyTyping == true)
        {
            CompleteText();
            StopAllCoroutines(); //cuidao q paramos todas
            isCurrentlyTyping = false;
            return;
        }

        if (dialogueInfo.Count == 0)
        {
            EndofDialogue();
                return;
        }

        


        DialogueBase.Info info = dialogueInfo.Dequeue(); //recoge el primer dialogo
        completeText = info.myText;


        dialogueName.GetComponent<TextMeshPro>().text = info.character.myName;
        dialogueText.GetComponent<TextMeshPro>().text = info.myText;
        dialogueText.GetComponent<TextMeshPro>().font = info.character.myFont.gameObject.GetComponent<TextMeshPro>().font;
        info.ChangeEmotion();
        dialoguePortrait.sprite = info.character.MyPortrait;

        dialogueText.GetComponent<TextMeshPro>().text = "";
      StartCoroutine(TypeText(info));

    }

    private bool CheckPunctuation(char c)
    {
        if (punctuationCharacters.Contains(c))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator TypeText(DialogueBase.Info info)
    {
        isCurrentlyTyping = true;
        
        foreach(char c in info.myText.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.GetComponent<TextMeshPro>().text += c;
            AudioManager.instance.PlayClip(info.character.myVoice);

            if(CheckPunctuation(c))
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        isCurrentlyTyping = false;
    }

    private void CompleteText()
    {
        dialogueText.GetComponent<TextMeshPro>().text = completeText;
    }

   
    

    public void EndofDialogue()
    {

        dialogueBox.SetActive(false);
    }

   
   

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inDialogue)
            {
                DequeueDialogue();
            }
        }
    }
    */
}