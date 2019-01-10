﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour {

  public int playID;
  public int numSources;


    public static AudioPlayer instance = null; //{ get; private set; }
    
    public float db;

    public AudioMixerGroup output;
    public AudioMixer mixer;
    private static AudioPlayer _instance;

    private GameObject[] objects;
    private AudioSource[] sources;



void Update(){
//    mixer.SetFloat("Volume",db);
}


    void Awake() {

        if( instance == null ){
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }else if( instance != this ){
            Destroy( gameObject);
        }

        DontDestroyOnLoad( gameObject );

        sources = new AudioSource[numSources];
        objects = new GameObject[numSources];

        for( int i = 0; i < numSources; i++){
            objects[i] = new GameObject();
            objects[i].transform.parent = transform;

            sources[i] = objects[i].AddComponent<AudioSource>() as AudioSource;
            sources[i].dopplerLevel = 0;
            sources[i].playOnAwake = false;
            sources[i].outputAudioMixerGroup = output;
        }
    }

    public void Play( AudioClip clip ){

        sources[playID].clip = clip;
        sources[playID].Play();

        playID ++;
        playID %= numSources;
    }


    public void Play( AudioClip clip , float pitch){

        sources[playID].volume = 1;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , float pitch , float volume){

        sources[playID].volume = volume;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , int step , float volume ){

        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        Play(clip);
    }



      public void Play( AudioClip clip , int step , float volume , Vector3 location , float falloff ){

        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        sources[playID].spatialize = true;
        sources[playID].spatialBlend = 1;
        sources[playID].maxDistance = falloff;
        sources[playID].minDistance = falloff/10;

        objects[playID].transform.position = location;
        Play(clip);
    }
}
