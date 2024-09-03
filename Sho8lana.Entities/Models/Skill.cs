﻿namespace Sho8lana.Entities.Models
{
	public class Skill
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public List<Freelancer>? Freelancers { get; set; }
		public List<Project>? Projects { get; set; }
	}
}
