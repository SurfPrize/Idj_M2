using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Follower : MonoBehaviour
{
    public static Floor_Follower Floor;

    private void Awake()
    {
        if (Floor == null)
            Floor = this;
        else
            Destroy(this);

        StartCoroutine(Change_pos());
    }

    public IEnumerator Change_pos()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("moving...");
        transform.position = new Vector3(PlayerMovement.current.transform.position.x, 0, PlayerMovement.current.transform.position.z);

        StartCoroutine(Change_pos());
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
