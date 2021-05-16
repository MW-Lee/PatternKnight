using UnityEngine;
using System.Collections;

public class CloudSystem : MonoBehaviour
{
    public int whatCloud;

    public float fCloudPosX;
    public float fEndPosX;
    public float minCloudPosY;
    public float maxCloudPosY;

    public GameObject propInstance;


	void Start ()
    {
        //Random.seed = System.DateTime.Now.Millisecond;
        createCloud();
	}

    void createCloud()
    {
        float posY = Random.Range(minCloudPosY, maxCloudPosY);

        whatCloud = Random.Range(1, 4);

        Vector3 CloudPos = new Vector3(fCloudPosX, posY, transform.position.z);

        //propInstance = (GameObject)Instantiate("Cloud_" + whatCloud, CloudPos, Quaternion.identity);
        GameObject temp = GameObject.Find("cloud" + whatCloud.ToString());
        propInstance = (GameObject)Instantiate(temp, CloudPos, Quaternion.identity);

        float speed = Random.Range(1, 2);
        speed *= -1;

        propInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

	
	void Update ()
    {
		if(propInstance)
        {
            if(propInstance.transform.position.x < fEndPosX)
            {
                Destroy(propInstance);
                createCloud();
            }
        }
	}
}
