using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float hitsToDestroy;
    float hits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits++;
        if (hits >= hitsToDestroy) Destroy(this.gameObject);
    }
}
