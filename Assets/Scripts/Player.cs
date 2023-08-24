using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;

    private const float maxX = 2.60f;
    private const float minX = -2.65f;

    //private float speed = 3f;
    private bool isShooting;
    //private float cooldown = 0.5f;
    [SerializeField] private ObjectPool objectPool = null;

    public ShipStats shipStats;

    private Vector2 offSecreenPos = new Vector2(0, -20f);
    private Vector2 startPos = new Vector2(0, -6f);

    private void Start()
    {
        shipStats.currentHealth = shipStats.maxHealth;
        shipStats.currentLifes = shipStats.maxLifes;
        transform.position = startPos;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A) && transform.position.x > minX)
        {
            transform.Translate(Vector2.left * Time.deltaTime * shipStats.shipSpeed);

        } 
        if (Input.GetKey(KeyCode.D) && transform.position.x < maxX)
        {
            transform.Translate(Vector2.right * Time.deltaTime * shipStats.shipSpeed);
        }
        if (Input.GetKey(KeyCode.Space) && !isShooting)
        {
            StartCoroutine(Shoot());
        }

#endif
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        //Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = gameObject.transform.position;
        yield return new WaitForSeconds(shipStats.fireRate);
        isShooting = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("hit");
            collision.gameObject.SetActive(false);
            TakeDamage();
        }
    }

    private IEnumerator Respawn()
    {

        transform.position = offSecreenPos;

        yield return new WaitForSeconds(2);

        shipStats.currentHealth = shipStats.maxHealth;
    
        transform.position = startPos;
    }



    private void TakeDamage()
    {
        shipStats.currentHealth--;

        if (shipStats.currentHealth <= 0)
        {
            shipStats.currentLifes--;
        
            if (shipStats.currentLifes <= 0)
            {   
                Debug.Log("Game Over");
            }
            else
            {
                Debug.Log("Respawn");
                StartCoroutine(Respawn());
            }
        }
        

    }

}
