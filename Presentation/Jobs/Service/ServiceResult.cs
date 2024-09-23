namespace Presentation.Jobs.Service
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ServiceResult SuccessResult(string message = "")
        {
            return new ServiceResult { Success = true, Message = message };
        }

        public static ServiceResult Failure(string message)
        {
            return new ServiceResult { Success = false, Message = message };
        }
    }

}
