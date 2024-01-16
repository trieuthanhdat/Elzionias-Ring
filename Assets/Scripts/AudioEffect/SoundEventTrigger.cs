using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Audio
{
    public class SoundEventTrigger : MonoBehaviour
    {
        public UnityEvent OnEventStart;
        private void OnTriggerEnter(Collider other)
        {
            
            if(other.CompareTag("Player"))
            {
                OnEventStart.Invoke();
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
  
        
    }
    
}
