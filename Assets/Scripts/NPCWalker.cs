using UnityEngine;

public class NPCWalker : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float waitTimeAtWaypoint = 2f;
    [SerializeField] private float stoppingDistance = 0.3f;

    private Transform[] waypoints;
    private int currentWaypoint;
    private float waitTimer;
    private bool waiting;

    public void SetWaypoints(Transform[] pts)
    {
        waypoints = pts;
        currentWaypoint = Random.Range(0, pts.Length);
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }
            return;
        }

        Transform target = waypoints[currentWaypoint];
        
        // Only move on X and Z, ignore Y so it doesn't fly/sink
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = (targetPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > stoppingDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction); // face movement direction
        }
        else
        {
            waiting = true;
            waitTimer = waitTimeAtWaypoint;
        }
    }
}