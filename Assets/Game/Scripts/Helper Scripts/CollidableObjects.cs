using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObjects : MonoBehaviour
{
    private PlayerRunner playerRunner;

    public ObjectType objectType;

    public enum ObjectType { 
        Money,
        Bottle,
        FinishLine,
        Atm
    }

    private void Start()
    {
        playerRunner = FindObjectOfType<PlayerRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectType == ObjectType.Money) {
            var particle = ObjectPooler.Instance.GetPooledObject("MoneyParticle");
            particle.transform.position = other.gameObject.transform.position + new Vector3(0f, 0.75f, 0.5f);
            particle.transform.rotation = gameObject.transform.rotation;
            particle.SetActive(true);
            particle.GetComponent<ParticleSystem>().Play();

            PlayerManagement.Instance.AddMoney(10);
            gameObject.SetActive(false);
        }
        if (objectType == ObjectType.Bottle)
        {
            var particle = ObjectPooler.Instance.GetPooledObject("BottleParticle");
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.SetActive(true);
            particle.GetComponent<ParticleSystem>().Play();

            PlayerManagement.Instance.AddMoney(-10);
            gameObject.SetActive(false);
        }
        if (objectType == ObjectType.Atm) {
            gameObject.SetActive(false);
            var particle = ObjectPooler.Instance.GetPooledObject("AtmParticle");
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.SetActive(true);
            particle.GetComponent<ParticleSystem>().Play();

            PlayerManagement.Instance.AddMoney(-15);
            playerRunner.DodgeBack();
        }
        if (objectType == ObjectType.FinishLine) {
            playerRunner.SetEnabled(false);
            Invoke("FinishedAction", 1.5f);
            Debug.Log("Touched FinishLine");
        }


    }
    void FinishedAction() {
        
        PlayerManagement.Instance.StartToDance();
        UIManager.Instance.NextLvlUI();
    }

}
