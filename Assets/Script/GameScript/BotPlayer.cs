using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BotPlayer : MonoBehaviour{
    public List<Disc> discsInPlayerSide = new();

    void Start()
    {
        StartCoroutine(PushDiscsFrequently());
    }

    IEnumerator PushDiscsFrequently()
    {
        yield return new WaitForSeconds(3);
        PushRandomDisc();
        StartCoroutine(PushDiscsFrequently());
    }

    public void PushRandomDisc(){
        if(discsInPlayerSide.Count == 0){
            return;
        }
        Disc disc = discsInPlayerSide[Random.Range(0, discsInPlayerSide.Count)];
        disc.GrabDisc();
        disc.SetBotTarget(new Vector3(1,0.7f,1));
        disc.PushDisc();
    }

    public void AddDisc(Disc disc){
        discsInPlayerSide.Add(disc);
    }

    public void RemoveDisc(Disc disc){
        discsInPlayerSide.Remove(disc);
    }
}