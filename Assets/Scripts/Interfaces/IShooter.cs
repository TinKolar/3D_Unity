public interface IShooter
{
        int GetShooterId(); // usually returns GetInstanceID()
        void AssignBulletPool(BulletPoolManager manager);

}
