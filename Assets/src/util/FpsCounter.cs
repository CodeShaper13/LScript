using UnityEngine;
using System;

public class FpsCounter : MonoBehaviour {

    /// <summary> The number of frames per second </summary>
    private int framesPerSecond = 0;
    /// <summary> The current frame count </summary>
    private int frameCount = 0;
    /// <summary> The frames timer </summary>
    private DateTime m_timerFrames = DateTime.MinValue;

    private void OnGUI() {
        if(m_timerFrames < DateTime.Now) {
            framesPerSecond = frameCount;
            frameCount = 0;
            m_timerFrames = DateTime.Now + TimeSpan.FromSeconds(1);
        }
        ++frameCount;
        GUILayout.Label(string.Format("Frames per second: {0}", framesPerSecond));
    }
}
