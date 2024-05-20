using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using AudioController;

public class DEBUG_ControlLoop : MonoBehaviour
{
    [SerializeField] private EventReference _eventRef;
    private EventInstance _event;

    private void Start()
    {
        _event = AudioManager.GetAudioManager().CreateEventInstance(_eventRef);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _event.getPlaybackState(out var state);
            if (state.Equals(PLAYBACK_STATE.STOPPED) || state.Equals(PLAYBACK_STATE.STOPPING))
            {
                _event.start();
            }
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            _event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
