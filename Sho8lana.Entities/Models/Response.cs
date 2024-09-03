
namespace Sho8lana.Entities.Models
{
	public class Response
	{
        public Response() {}
		/// <summary>
		/// Success response
		/// </summary>
		/// <param name="stsCode"></param>
		public Response(int stsCode, Object? result = null)
        {
            this.StatusCode = stsCode;
			this.IsSuccess = true;
			this.Errors = [];
			this.Result = result;
        }
		/// <summary>
		/// Failed response
		/// </summary>
		/// <param name="stsCode"></param>
		/// <param name="Result"></param>
		public Response(int stsCode, List<string> errors)
        {
            this.StatusCode = stsCode;
			this.IsSuccess = false;
			this.Errors = errors;
        }
        public int StatusCode { get; set; }
		public bool IsSuccess { get; set; }
		public List<string>? Errors { get; set; }
		public Object? Result { get; set; }
	}
}
