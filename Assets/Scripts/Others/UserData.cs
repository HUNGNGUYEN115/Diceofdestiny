using UnityEngine;
using System.Collections;
using TMPro;

public class UserData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UserData instance;
    public TMP_InputField name;
    private string playerName;
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        playerName = name.text;
    }
}
