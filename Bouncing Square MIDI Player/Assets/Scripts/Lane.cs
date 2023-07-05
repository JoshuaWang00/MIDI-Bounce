using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lane : MonoBehaviour
{
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<GameObject> laneCoords = new List<GameObject>();
    public BounceSquare mainSquare;
    public GameObject rectPrefab;
    int bounceIndex = 1;
    public float changingSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        changingSpeed = Mathf.Sqrt((BounceSquare.xspeed * BounceSquare.xspeed) + (BounceSquare.xspeed * BounceSquare.xspeed));
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        MetricTimeSpan prevTimeStamp = TimeConverter.ConvertTo<MetricTimeSpan>(array[0].Time, SongManager.midiFile.GetTempoMap());
        foreach (var note in array)
        {

            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
           if (prevTimeStamp != metricTimeSpan)
            {
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
            prevTimeStamp = metricTimeSpan;
        }
    }

    public void setBounceLane(List<double> listTimeStamps)
    {
        double prevx = 0;
        double prevy = 0;
        double prevTimeStamp = 0;
        int counter = 0;
        float currentx = 0;
        float currenty = 0;




        foreach (var timestamp in listTimeStamps)
        {
            //upper bounce rects
            if (counter % 2 == 0)
            {


                currentx = (float)(prevx + (BounceSquare.xspeed * (timestamp - prevTimeStamp)));
                currenty = (float)(prevy + (BounceSquare.yspeed * (timestamp - prevTimeStamp)) + 0.01);
                GameObject newRect = (GameObject)Instantiate(rectPrefab, new Vector3(currentx, currenty,0), Quaternion.identity);
                laneCoords.Add(newRect);
                prevx = (float)prevx + (BounceSquare.xspeed * (timestamp - prevTimeStamp));
                prevy = (float)prevy + (BounceSquare.yspeed * (timestamp - prevTimeStamp));
                prevTimeStamp = timestamp;
                counter += 1;
            }
            //lower bounce rects
            else if (counter % 2 == 1)
            {
                currentx = (float)(prevx + (BounceSquare.xspeed * (timestamp - prevTimeStamp)));
                currenty = (float)(prevy - (BounceSquare.yspeed * (timestamp - prevTimeStamp)) - 0.01);
                GameObject newRect = (GameObject)Instantiate(rectPrefab, new Vector3(currentx,currenty,0), Quaternion.identity);
                laneCoords.Add(newRect);
                prevx = (float)prevx + (BounceSquare.xspeed * (timestamp - prevTimeStamp));             
                prevy = (float)prevy - (BounceSquare.yspeed * (timestamp - prevTimeStamp));

                prevTimeStamp = timestamp;
                counter += 1;
            }


            
        }
        
    }
    // Update is called once per frame


    void Update()
    {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene("StartMenu");
        }


        if (bounceIndex < timeStamps.Count)
        {

            mainSquare.transform.position = Vector2.MoveTowards(mainSquare.transform.position, laneCoords[bounceIndex - 1].transform.position, changingSpeed * Time.deltaTime);
            if (SongManager.GetAudioSourceTime() >= timeStamps[bounceIndex - 1])
            {
                
                bounceIndex++;
            }

            

        }

         else { mainSquare.transform.position = Vector2.MoveTowards(mainSquare.transform.position, laneCoords[bounceIndex-1].transform.position, changingSpeed * Time.deltaTime); }


    }
}