namespace Deblue.Data
{
    public class PersistentDataHolder<T> : PersistentMono<PersistentDataHolder<T>>
    {
        public T Data => _data;

        protected T _data;

        protected string _fileName = typeof(T).Name;

        protected void SaveData()
        {
            SavingManager.Save(_fileName, _data);
        }

        protected T LoadData()
        {
            _data = SavingManager.Load(_fileName, default(T));
            return _data;
        }
    }
}