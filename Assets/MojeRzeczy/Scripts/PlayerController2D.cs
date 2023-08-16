using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController2D : MonoBehaviour
{

    Rigidbody2D rb;
    CapsuleCollider2D PlayerCollider;
    public GameObject MiddleText;
    public float RotationSpeed = 200;
    public float InitialForwardSpeed = 50000;
    public float MinForwardSpeed = 40000;
    public float ForwardSpeed; //
    public float ForwardRate = 0.3f; //w sekundach
    public Vector2 SizeSwiming;
    public Vector2 SizeWalking;
    public GameObject ColliderLayer;
    public float InitialGroundSpeed = 1000;
    public float MinGroundSpeed = 500;
    public float GroundSpeed;//
    public bool OnGround = false;
    //Zdrowie
    public int health = 3;
    float throwbackPower = 500;
    //Do kontroli głodu
    private int FishNum=0;
    private float Hunger;
    public float HungerGrowSpeed=1;
    public float StartHunger = 0;
    public float MaxHunger =100;
    public float FishHungerDrop=15;
    private float rotation = 0;
    Vector2 ForwardVector;
    private bool forward = false;
    private float LastForwardTime = 0;
    private float diference;
    private float horizontal;
    private float vertical;
    private float angle;
    Animator anim;
    public GameObject DeadDuck;
    public bool CanDealDamage = true;
    // Update is called once per frame

    void Start()
    {
        ForwardVector.x = 0;
        ForwardVector.y = 1;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        //ColliderLayer = GameObject.Find("GroundTilemapOutside");
        Hunger = StartHunger;
        ForwardSpeed = InitialForwardSpeed;
        GroundSpeed = InitialGroundSpeed;
        SizeSwiming.x = 0.4f;
        SizeSwiming.y = 0.9f;
        SizeWalking.x = 0.4f;
        SizeWalking.y = 0.4f;

        MiddleText.GetComponent<Text>().text = "";
    }

    void Update()
    {
        //INPUT Movement
        if (OnGround == false)
        {
            rotation = Input.GetAxis("Rotate") * RotationSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                forward = true;
            }
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        //INPUT FISH
        if (Input.GetButtonDown("FishEat"))
        {
                EatFish();
        }
        //ANIMATOR
        anim.SetFloat("Speed", rb.velocity.magnitude);
        anim.SetFloat("AnimationSpeed", rb.velocity.magnitude+5);

        //HUNGER
        if (Hunger < MaxHunger)
        {
            HungerChange(HungerGrowSpeed * Time.deltaTime);
        }
        else GameOver();
    }
    void FixedUpdate()
    {
        //MOVEMENT
      
            if (OnGround == false)
            {
                transform.Rotate(0, 0, -rotation * Time.fixedDeltaTime);
                diference = Time.time - LastForwardTime;
                anim.SetFloat("TimeFromLastSwim", diference);
                if (forward)
                {
                    if (diference > ForwardRate)
                    {
                        anim.SetBool("Forward", true);
                        rb.AddRelativeForce(ForwardVector * ForwardSpeed*Time.fixedDeltaTime);
                        LastForwardTime = Time.time;
                    }
                    forward = false;
                }
                else anim.SetBool("Forward", false);
            }
            else
            {
                Vector2 trans;
                trans.x = horizontal * Time.fixedDeltaTime * GroundSpeed;
                trans.y = vertical * Time.fixedDeltaTime * GroundSpeed;
                rb.AddForce(trans);
                if (rb.velocity.magnitude > 0.5)
                {
                    Vector2 v = rb.velocity;
                    angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                }
            }
    }
    public void GetOnGround()
    {
        //Nadanie widoczności
        GetComponent<SpriteRenderer>().sortingLayerName = "GroundLevel";
        //Wyłączenie colidera ziemii
        ColliderLayer.GetComponent<Collider2D>().enabled = false;
        OnGround = true;
        anim.SetBool("OnGround", true);
        //zmiana collidera
        PlayerCollider.size = SizeWalking; 

    }
    public void GetOffGround()
    {
        if (OnGround == true)
        {
            //Nadanie widoczności
            GetComponent<SpriteRenderer>().sortingLayerName = "SeaLevel";
            //Włączenie colidera
            ColliderLayer.GetComponent<Collider2D>().enabled = true;
            OnGround = false;
            anim.SetBool("OnGround", false);
            rb.AddRelativeForce(ForwardVector*0.5f);
            //zmiana collidera
            PlayerCollider.size = SizeSwiming;
        }
    }
    private void EatFish()
    {
        if (FishNum > 0)
        {
            FishNum = FishNum - 1;
            HungerChange(-FishHungerDrop);
            Debug.Log("You ate fish");
        }
    }
    public void FishPickUp(int PickUpAmount)
    {
        FishNum = FishNum + PickUpAmount;
    }
    public void EatChild(float ChildCallories)
    {
        HungerChange(-ChildCallories);
        Debug.Log("You ate fetus");
    }
    public float GetHunger()
    {
        return Hunger;
    }
    public int GetFish()
    {
        return FishNum;
    }
    public void GameOver()
    {
        Debug.Log("Umarłeś");
        MiddleText.GetComponent<Text>().text = "GAME OVER";
        Instantiate(DeadDuck,transform.position,Quaternion.identity);
        Destroy(gameObject);
        //GetComponent<SpriteRenderer>().enabled = false;
    }
    void HungerChange(float change)
    {
        Hunger = Hunger + change;
        if (Hunger < 0) Hunger = 0;
        GroundSpeed = MinGroundSpeed + ((MaxHunger - Hunger)/MaxHunger) * (InitialGroundSpeed-MinGroundSpeed);
        ForwardSpeed = MinForwardSpeed + ((MaxHunger - Hunger) / MaxHunger) * (InitialForwardSpeed - MinForwardSpeed);
    }
    public void EndLevel()
    {
        Debug.Log("Koniec Poziomu");
        MiddleText.GetComponent<Text>().text = "Koniec poziomu\n\n Ryby:"+FishNum;
        Destroy(gameObject);
        //GetComponent<SpriteRenderer>().enabled = false;
    }
    public void DealDamage(int dmg, Vector2 throwback)
    {
        if (CanDealDamage)
        {
            health = health - dmg;
            if (health <= 0) GameOver();
            rb.AddForce(throwback * throwbackPower);
            Debug.Log("DamageDealt");
            CanDealDamage = false;
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("ImmunityEnd", 0.5f);
        }
    }
    void ImmunityEnd()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        CanDealDamage = true;
    }
}
