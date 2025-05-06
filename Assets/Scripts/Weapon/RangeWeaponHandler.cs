using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    // 총알에 인덱스를 가져와서 어떤 총알을 사용할 건지 정해 줌
    [SerializeField] private int bulletIndex;
    public int BulletIndex {get {return bulletIndex;}}

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize {get {return bulletSize;}}

    // 총알이 날아가서 얼마나 살아있을 건지
    [SerializeField] private float duration;
    public float Duration {get {return duration;}}

    [SerializeField] private float spread;
    public float Spread {get {return spread;}}

    [SerializeField] private int numberofProjectilesPerShot;
    public int NumberofProjectilesPerShot {get {return numberofProjectilesPerShot;}}

    // 각각 탄의 퍼짐 정도도
    [SerializeField] private float multipleProjectileAngle;
    public float MultipleProjectileAngle {get {return multipleProjectileAngle;}}

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor {get {return projectileColor;}}

    private ProjectileManager projectileManager;

    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot;

        // 발사해야 하는 최소 각도
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++) {
            // 각각의 개수(번호)만큼 더 이동해서 쏜다
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = UnityEngine.Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle) {
        projectileManager.ShootBullet(this, projectileSpawnPosition.position, 
                                        RotateVector2(_lookDirection, angle));

    }

    private static Vector2 RotateVector2 (Vector2 v, float degree) {
        // Quaternion이 가지고 있는 회전 수치만큼 Vector를 회전시켜 준다
        // Vector 자체에 회전을 적용할 때 Quaternion을 곱해 준다
        return Quaternion.Euler(0, 0, degree) * v;
    } 
}
