using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField speedInput;
    public string mp3Path;
    public static AudioClip songToPlay;
    public static string midPath;
    //first flag is is valid speed, second is valid mp3, third is valid mid
    public bool[] validFlags = new bool[]{true, false, false};
    public Text notifString;
    public void StartScene()
    {
        foreach (bool flag in validFlags) { if (flag == false) {
                notifString.text = "Missing set values!";
               return;
            } }
        notifString.text = "";
        SceneManager.LoadScene("BounceScene");
        
    }

    public void quit() {
        Application.Quit();
    }

    public void SetSpeed() {
        BounceSquare.xspeed = int.Parse(speedInput.text);
        BounceSquare.yspeed = int.Parse(speedInput.text);
    }

    public Text speedText;
    public void SetSpeedText() {
        if (int.Parse(speedInput.text) < 1) { speedText.text = "Speed: Invalid! Must be greater than 0!";
            validFlags[0] = false;
        }
        else { speedText.text = "Speed: " + System.Convert.ToString(BounceSquare.xspeed);
            validFlags[0] = true;
        }
    }

    public Text mp3Text;
    string[] mp3FileArr;
    char[] splitOptions = { '\\', '/' };
    public void SetMp3() {
        var mp3bp = new BrowserProperties();
        mp3bp.filter = "Audio files (*.mp3) | *.mp3";
        new FileBrowser().OpenFileBrowser(mp3bp, path =>{                      
            mp3Path = path;
            Debug.Log(mp3Path);
        });
        mp3FileArr = mp3Path.Split(splitOptions);
        mp3Text.text = "MP3 Path: ...\\" + mp3FileArr[mp3FileArr.Length-1];
        validFlags[1] = true;
        StartCoroutine(GetAudioClip());
        
    }




    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(mp3Path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                songToPlay = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    public Text midiText;
    string[] midiFileArr;
    public void SetMidi() {
        var midbp = new BrowserProperties();
        midbp.filter = "Audio files (*.mid) | *.mid";
        validFlags[2] = true;
        new FileBrowser().OpenFileBrowser(midbp, path => {
            
            midPath = path;
            
        });

        midiFileArr = midPath.Split(splitOptions);
        midiText.text = "MIDI Path: ...\\" + midiFileArr[midiFileArr.Length - 1];
        validFlags[2] = true;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
