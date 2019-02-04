using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBulletFactory : MonoBehaviour
{
    public abstract List<GameObject> CreateBulletPool();

	
}
