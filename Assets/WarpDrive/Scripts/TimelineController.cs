using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;
using UnityEngine.Playables;


public class TimelineController : MonoBehaviour

{
    /*private OscServer server;
    public PlayableDirector director;

    void Start()
    {
        server = new OscServer(7000); // Listen on port 7000
        server.MessageDispatcher.AddCallback("/timeline", OnTimelineMessage);
    }

    void OnTimelineMessage(OscMessage message)
    {
        float time = message.GetFloat(0);
        director.time = time;
    }

    void OnDestroy()
    {
        server.Dispose();
    }*/
}
