using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private WeaponsSo weaponsSo;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string[] hitTags;
    private Vector3 _firstPosition;

    private GameObject _owner;

    public void Init(WeaponsSo so, GameObject owner)
    {
        weaponsSo = so;
        _owner = owner;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _firstPosition= transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(_firstPosition, transform.position) >= weaponsSo.range)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        bool tagOk = hitTags == null || hitTags.Length == 0;
        if (!tagOk)
        {
            foreach (var hitTag in hitTags)
            {
                if (other.CompareTag(hitTag)) { tagOk = true; break; }
            }
        }
        if (!tagOk) return;
        var info = new DamageInfo(
            weaponsSo.damage,
            _owner ? _owner : gameObject,
            transform.position,
            -transform.forward
        );
        
            if (other.TryGetComponent(out IHittableProp hittable))
            {
                hittable.OnHit();
                Destroy(gameObject); 
            }
            if (other.TryGetComponent(out IGetHit damageable))
            {
                damageable.GetHit(info);
                Destroy(gameObject); 
            }
    }
}