
namespace src.Models
{
	public class Response
	{
        public Response() {}
        public Response(int sts, bool success, List<string> errors)
        {
            this.StatusCode = sts;
			this.Result = success;
			this.Errors = errors;
        }
		public Response(int sts)
        {
            this.StatusCode = sts;
			this.IsSuccess = true;
        }
		public Response(int sts, Object? Result)
        {
            this.StatusCode = sts;
			this.IsSuccess = true;
			this.Result = Result;
        }
        public int StatusCode { get; set; }
		public bool IsSuccess { get; set; }
		public List<string>? Errors { get; set; }
		public Object? Result { get; set; }
	}
}
