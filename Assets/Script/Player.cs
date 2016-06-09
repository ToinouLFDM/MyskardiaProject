using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    bool isDead = false;

    
    [SyncVar]
    float timer;

    [SyncVar]
    double time = -1;

    
    public bool IsDead
    {
        get { return isDead; }
        protected set { isDead = value; }
    }

    [SerializeField]
    int maxHealth = 100;

    

   

	[SerializeField]
	GameObject graphics;

	

   

  

    
  

    [SerializeField]
    Behaviour[] disableOnDeath;

    [SerializeField]
    MeshRenderer[] disableGraphicsOnDeath;

    [SerializeField]
    ParticleSystem deadBody;

    bool[] wasEnabled;

    

    [SyncVar]
    int currentHealth;

  

    

  
  

    public int Life
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

   
    public float Timer { get { return timer; }
                        set { timer = value; } 
    }

    public double TimeStarted
    {
        get { return time; }
        set { time = value; }
    }

    public void Setup()
    {
    
        

        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }


      
        SetDefaults();
    }

    void Update()
    {
       
        timer +=0.0166f;
        if (maxHealth<currentHealth)
        {
           
            currentHealth = maxHealth;
        }
       
        
    }

   

    [ClientRpc]
    public void RpcGetDamage(int _damage, bool sucide)
    {

        
        if (isDead)
            return;

       
       
        
            currentHealth -= _damage;
        

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            
           
                Die();
                
               
           

            
        }
        
        GetDamageHUD();

        Debug.Log(transform.name + " have now " + currentHealth + " / " + maxHealth);
    }

 

   

    private IEnumerator EnablePlayer(Player p, int cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        if(p.gameObject.GetComponent<NetworkIdentity>().netId.Value != gameObject.GetComponent<NetworkIdentity>().netId.Value)
            p.graphics.SetActive(true);
    }


   

   

    public void Die()
    {
        isDead = true;
       
       
        foreach (var item in disableGraphicsOnDeath)
        {
            item.enabled = false;
        }

        //deadBody.enableEmission = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

      

        Debug.Log(transform.name + " is dead.");

        //Respawn
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManaer.instance.respawnTime);

        foreach (var item in disableGraphicsOnDeath)
        {
            item.enabled = true;
        }

        //deadBody.enableEmission = false;

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log("Player respawned");
    }

   

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
       

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

       
    }

    public void GetDamageHUD()
    {
        
           
    }
}
