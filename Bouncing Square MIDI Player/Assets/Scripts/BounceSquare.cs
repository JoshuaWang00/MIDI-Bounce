using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSquare : MonoBehaviour
{
   [SerializeField] public static int xspeed = 10;
    [SerializeField] public static int yspeed = 10;
    public Rigidbody2D rb;
    public Lane laneManager;
    public int count = 0;
    double timeInstantiated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
