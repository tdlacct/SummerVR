using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class ChangeArea : MonoBehaviour
{

    // Use this for initialization
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public Vector3 MovePos;
    public GameObject CurSphere;
    public GameObject NewSphere;
    public string keyword;
    // Use this for initialization
    void Start()
    {
        keywords.Add(keyword, () =>
        {
            Debug.Log("I hear you");
            GameObject.Find("Transport").transform.position = MovePos;
            CurSphere.SetActive(false);
            NewSphere.SetActive(true);
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
