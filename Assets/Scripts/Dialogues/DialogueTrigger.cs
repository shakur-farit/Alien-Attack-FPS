using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Has dialogue or not")]
    public bool hasDialogue = true;

    [Header("Do want to destroy object with dialogue after collid with it")]
    public bool isDestuctible = false;

    [Header("Do want to launch a dialogue after the time")]
    public bool hasTiming = false;
    [SerializeField] private Timer timer;
    [Tooltip("In how much seconds need to launch the dailogue")]
    [SerializeField] private float launchDialogueIn;

    [Header("Component Settings")]
    [SerializeField] private Dialogue dialogue;


    private void Update()
    {

        if (hasDialogue && hasTiming && timer.currentTime <= launchDialogueIn)
        {
            TriggerDialogue();
            Destroy(this.gameObject);
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (hasDialogue && collision.gameObject.tag == "Player")
        {

            if (this.gameObject.tag == "Door" && GetComponent<Door>().isOpen == false)
            {
                TriggerDialogue();
            }
            else if (this.gameObject.tag != "Door")
            {
                TriggerDialogue();
                if (isDestuctible)
                    Destroy(this.gameObject);
            }
        }
    }
}
