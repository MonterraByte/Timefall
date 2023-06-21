using UnityEngine;

public class CrushScript : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float limitLow = 0.0f;
    public float limitHigh = 1.0f;
    public float cooldown = 0.25f;

    private int state = 0;
    private float stay = 1.0f;
    private float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        switch(this.state)
        {
            case 0:
                this.transform.position += Vector3.down * this.moveSpeed * Time.deltaTime;

                if (this.transform.position.y <= this.limitLow)
                {
                    this.state = 1;
                }
                break;

            case 1:
                if (this.currentTime > this.stay)
                {
                    this.state = 2;
                    this.currentTime = 0.0f;
                }
                else
                {
                    this.currentTime += Time.deltaTime;
                }
                break;

            case 2:
                this.transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;

                if (this.transform.position.y >= this.limitHigh)
                {
                    this.state = 3;
                }
                break;

            case 3:
                if (this.currentTime > this.cooldown)
                {
                    this.state = 0;
                    this.currentTime = 0.0f;
                }
                else
                {
                    this.currentTime += Time.deltaTime;
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            player.TakeDamage(1, true);
        }
    }
}
