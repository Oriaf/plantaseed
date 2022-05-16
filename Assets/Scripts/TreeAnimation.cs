using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimation : MonoBehaviour
{
   public Animator _animator; 
   public GameObject _gameWonMessage;


   private void OnTriggerEnter(Collider other) {
       if(other.CompareTag("Player"))
       {
           triggerAnimation();
           StartCoroutine(DisplayGameWon());
       }
   }

   private void triggerAnimation()
   {
       _animator.SetTrigger("Play");
       
   }

   IEnumerator DisplayGameWon()
   {
       yield return new WaitForSeconds(7);
        _gameWonMessage.SetActive(true);

   }
}
