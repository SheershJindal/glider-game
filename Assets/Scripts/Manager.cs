using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { set; get; }

    public Material playerMaterial;
    public Color[] playerColors = new Color[10];
    public GameObject[] playerTrails = new GameObject[10];

    public int currentLevel = 0;
    public int menuFocus = 0;

    public AudioSource startMusic;

    private Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>();

    private void Start()
    {
        startMusic = GetComponent<AudioSource>();
        startMusic.Play(0);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public Vector3 GetPlayerInput()
    {
        if(SaveManager.Instance.state.usingAccelerometer)
        {
            Vector3 a = Input.acceleration;
            a.y = a.z;
            return a;
        }
        if(Input.touches.Length == 0)
        {
            activeTouches.Clear();
        }
        Vector3 r = Vector3.zero;
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                activeTouches.Add(touch.fingerId, touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if(activeTouches.ContainsKey(touch.fingerId))
                    activeTouches.Remove(touch.fingerId);
            }
            else
            {
                float mag = 0;
                r = (touch.position - activeTouches[touch.fingerId]);
                mag = r.magnitude / 300;
                r = r.normalized * mag;
                if(r.magnitude > 1)
                {
                    r.Normalize();
                }
            }
        }
        return r;
    }
}
