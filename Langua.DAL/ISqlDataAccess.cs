namespace Langua.DAL
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sp_cmd,U parameters);
        Task SaveData<T>(string sp_cmd, T parameters);
        Task<T> LoadDataById<T, U>(string storedProcedure, U parameters);


    }
}
