using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora;
using Agora.Rtc;

public class AgoraManager : MonoBehaviour
{
    public static AgoraManager Instance;

    internal IRtcEngine RtcEngine;
    private string appId = "1a500d1eb74641d3b46d085049e75f32";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = appId; 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
