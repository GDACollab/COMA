using UnityEngine;
using System.Collections;

public class makeNote : MonoBehaviour {

	public spawnA wrath;
	public spawnS sloth;
	public spawnD gluttony;
	public spawnF pride;
	public spawnJ envy;
	public spawnK greed;
	public spawnL lust;
	public spawnC chub;
    public AudioSource monstro;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Play ()
    {
        monstro.Play();
    }

	void MakerA () {
		wrath.ANoteS ();
	}
	void MakerS () {
		sloth.SNoteS ();
	}
	void MakerD(){
		gluttony.DNoteS ();
	}
	void MakerF(){
		pride.FNoteS ();
	}
	void MakerJ(){
		envy.JNoteS ();
	}
	void MakerK(){
		greed.KNoteS ();
	}
	void MakerL(){
		lust.LNoteS ();
	}
	void MakerC(){
		chub.CNoteS ();
	}
}
