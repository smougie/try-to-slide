using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    [SerializeField] private GameObject barTrigger = null;
    [SerializeField] private GameObject slidingWall = null;
    [SerializeField] private ParticleSystem dust = null;
    [SerializeField] private float slideSpeed = 1f;

    private Vector3 wallStartPosition;
    private Vector3 wallEndPosition;
    private Vector3 wallScale;
    private Vector3 dustShapeScale;
    private bool movingWall;

    private void Start()
    {
        var dustShape = dust.shape;
        wallScale = slidingWall.transform.localScale;
        dustShapeScale = new Vector3(wallScale.x + 0.3f, wallScale.z + 0.3f);
        dustShape.scale = dustShapeScale;
        wallStartPosition = slidingWall.transform.position;
        wallEndPosition = new Vector3(wallStartPosition.x, wallStartPosition.y - 1.1f, wallStartPosition.z);
    }
    private void Update()
    {
        if (movingWall)
        {
            slidingWall.transform.position = Vector3.MoveTowards(slidingWall.transform.position, wallEndPosition, slideSpeed * Time.deltaTime);
        }
        if (slidingWall.transform.position == wallEndPosition)
        {
            movingWall = false;
            Destroy(gameObject);
        }
    }

    public void BarTrigger()
    {
        Destroy(barTrigger);
        movingWall = true;
        dust.transform.position = new Vector3(slidingWall.transform.position.x, -.3f, slidingWall.transform.position.z);
        dust.Play();
    }

}
