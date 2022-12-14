using System;
using System.Threading;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;
using NAudio.Wave;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Sound;

public class GameEngineSound : Component, IDisposable
{ 
    [JsonProperty]public string audioFilePath { get; private set; }

    public override void Start(GameObject gameObject)
    {
        audioFilePath = "";
        base.Start(gameObject);
    }
    
    private NAudio.Wave.WaveFileReader wave = null;
    private NAudio.Wave.DirectSoundOut output = null;

    public void SetAudioFile(string path)
    {
        DisposeWave();
        
        wave = new NAudio.Wave.WaveFileReader(path);
        output = new NAudio.Wave.DirectSoundOut();
        output.Init(new NAudio.Wave.WaveChannel32(wave));
    }

    public void Play()
    {
        if(output != null)
            output.Play();
    }
    
    public void PlayPause()
    {
        if(output != null)
            if(output.PlaybackState == PlaybackState.Playing)
                output.Pause();
            else if(output.PlaybackState == PlaybackState.Paused)
                output.Play();
    }

    public void Pause()
    {
        if(output != null)
            output.Pause();
    }

    public void Stop()
    {
        if(output != null)
            output.Pause();
    }

    private void DisposeWave()
    {
        if (output != null)
        {
            if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
            output.Dispose();
            output = null;
        }
        if (wave != null)
        {
            wave.Dispose();
            wave = null;
        }
    }

    public override void SetObjectType()
    {
        ObjType = 8;
    }

    public void Dispose()
    {
        DisposeWave();
    }
}