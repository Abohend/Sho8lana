﻿namespace src.Models.Dto.Job
{
	public class CreateJobDto
	{
		public required string Description { get; set; }
		public required decimal Price { get; set; }
		//public required int ProjectId { get; set; } // take from url
		public string? FreelancerId { get; set; } // optional to enter
	}
}
