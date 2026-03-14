namespace Pasteleria.Shared.Extensions
{
    public class Result<T>
    {
        public Result(bool isSuccessful, T? data, List<string> errors)
        {
            IsSuccessful = isSuccessful;
            Data = data;
            Errors = errors;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, new List<string>());
        }

        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>(false, default(T), errors);
        }

        public T? Data { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }
    }
}
