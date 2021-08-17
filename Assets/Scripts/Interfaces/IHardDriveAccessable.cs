namespace Assets.Scripts.Interfaces
{
    interface IHardDriveAccessable
    {
        void SetString(string value);
        void SetInt(int value);
        void SetFloat(float value);
        string GetString();
        int GetInt();
        float GetFloat();
    }
}
