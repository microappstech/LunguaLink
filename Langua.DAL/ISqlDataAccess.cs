namespace Langua.DAL
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sp_cmd,U parameters);
        Task<List<T>> LoadData<T>(string sp_cmd);
        Task SaveData<T>(string sp_cmd, T parameters);
        Task<T> LoadDataById<T, U>(string storedProcedure, U parameters);


    }
}
