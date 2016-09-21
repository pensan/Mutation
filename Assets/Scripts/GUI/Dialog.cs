using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour
{
    public Text HeaderText;
    public Text ContentText;
    public Button OKButton;
    
    void Awake()
    {
        OKButton.onClick.AddListener(Hide);
    }

    public void Show(string header, string content, System.Action okAction = null)
    {
        this.gameObject.SetActive(true);

        HeaderText.text = header;
        ContentText.text = content;

        if (okAction != null)
            AddSaveListener(okAction);
    }


    private void AddSaveListener(System.Action listener)
    {
        UnityEngine.Events.UnityAction saveListener = null;
        saveListener = delegate ()
        {
            OKButton.onClick.RemoveListener(saveListener);
            listener();
        };
        OKButton.onClick.AddListener(saveListener);
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
