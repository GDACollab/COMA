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
	void EndBattle ()
	{
		Angel.TransitionFromBattleToField ();
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
    void DubAS()
    {
        wrath.ANoteD();
        sloth.SNoteD();
    }
    void DubAD()
    {
        wrath.ANoteD();
        gluttony.DNoteD();
    }
    void DubAF()
    {
        wrath.ANoteD();
        pride.FNoteD();
    }
    void DubAJ()
    {
        wrath.ANoteD();
        envy.JNoteD();
    }
    void DubAK()
    {
        wrath.ANoteD();
        greed.KNoteD();
    }
    void DubAL()
    {
        wrath.ANoteD();
        lust.LNoteD();
    }
    void DubAC()
    {
        wrath.ANoteD();
        chub.CNoteD();
    }
    void DubSD()
    {
        sloth.SNoteD();
        gluttony.DNoteD();
    }
    void DubSF()
    {
        sloth.SNoteD();
        pride.FNoteD();
    }
    void DubSJ()
    {
        sloth.SNoteD();
        envy.JNoteD();
    }
    void DubSK()
    {
        sloth.SNoteD();
        greed.KNoteD();
    }
    void DubSL()
    {
        sloth.SNoteD();
        lust.LNoteD();
    }
    void DubSC()
    {
        sloth.SNoteD();
        chub.CNoteD();
    }
    void DubDF()
    {
        gluttony.DNoteD();
        pride.FNoteD();
    }
    void DubDJ()
    {
        gluttony.DNoteD();
        envy.JNoteD();
    }
    void DubDK()
    {
        gluttony.DNoteD();
        greed.KNoteD();
    }
    void DubDL()
    {
        gluttony.DNoteD();
        lust.LNoteD();
    }
    void DubDC()
    {
        gluttony.DNoteD();
        chub.CNoteD();
    }
    void DubFJ()
    {
        pride.FNoteD();
        envy.JNoteD();
    }
    void DubFK()
    {
        pride.FNoteD();
        greed.KNoteD();
    }
    void DubFL()
    {
        pride.FNoteD();
        lust.LNoteD();
    }
    void DubFC()
    {
        pride.FNoteD();
        chub.CNoteD();
    }
    void DubJK()
    {
        envy.JNoteD();
        greed.KNoteD();
    }
    void DubJL()
    {
        envy.JNoteD();
        lust.LNoteD();
    }
    void DubJC()
    {
        envy.JNoteD();
        chub.CNoteD();
    }
    void DubKL()
    {
        greed.KNoteD();
        lust.LNoteD();
    }
    void DubKC()
    {
        greed.KNoteD();
        chub.CNoteD();
    }
    void DubLC()
    {
        lust.LNoteD();
        chub.CNoteD();
    }
}
