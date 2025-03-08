using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class handling bot behavior on the game
/// </summary>
public class BotPlayer : MonoBehaviour {
    /// <summary>
    /// The disc objects list in the bot side
    /// </summary>
    private List<Disc> _discsInBotSide = new();

    void Start()
    {
        StartCoroutine(PushDiscsFrequently());
    }

    /// <summary>
    /// Every 3 seconds, the bot push a random disc in his side
    /// </summary>
    IEnumerator PushDiscsFrequently()
    {
        yield return new WaitForSeconds(3);
        if(_discsInBotSide.Count == 0){
            // Stop the interaction loop
            yield return null;
        }else{
            PushRandomDisc();
            StartCoroutine(PushDiscsFrequently());
        }
    }

    /// <summary>
    /// Grab and push a random disc in bot side, always in the same direction
    /// </summary>
    public void PushRandomDisc(){
        Disc disc = _discsInBotSide[Random.Range(0, _discsInBotSide.Count)];
        disc.GrabDisc();
        disc.SetBotTarget(new Vector3(1.0f, 0.7f, 1.0f));
        disc.PushDisc();
    }

    /// <summary>
    /// Add a disc positioned in the bot side
    /// </summary>
    /// <param name="disc">The new disc in the bot side</param>
    public void AddDisc(Disc disc){
        _discsInBotSide.Add(disc);
    }

    /// <summary>
    /// Remove a disc that is no more positioned in the bot side
    /// </summary>
    /// <param name="disc">The disc that leaved the bot side</param>
    public void RemoveDisc(Disc disc){
        _discsInBotSide.Remove(disc);
    }
}