using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandMirror : BaseItem
{
    [SerializeField] StatusGoldReceiveChange _statusPrefab;

    //Tên cũ là Xí ngầu 20 xu
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        return false;
    }

    public override bool Activate(string activeCase)
    {

        Owner.RemoveItem(this);
        // player active animation
        Destroy(gameObject);

        // Add item back to Item pool
        ItemManager.Ins.AddItemToPool(this);

        // Create status
        var newStatus = Instantiate(_statusPrefab, Owner.transform);
        newStatus.goldReceiveChange = 1f;
        newStatus.targetPlayer = Owner;
        newStatus.StartListen();

        return base.Activate(activeCase);
    }

    // Start is called before the first frame update
    void Start()
    {
        CanActivate = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
