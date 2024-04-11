using src.Data;
using src.Models;
using src.Models.Dto;

namespace src.Repository
{
	public class SkillRepository
	{
		private readonly Context _db;

		public SkillRepository(Context db)
		{
			this._db = db;
		}
		/// <summary>
		/// return false if Name already exist, true if created
		/// </summary>
		/// <param name="skill"></param>
		public bool Create(Skill skill)
		{
			var duplicatedSkill = _db.Skills.FirstOrDefault(s => s.Name == skill.Name);
			if (duplicatedSkill == null)
			{
				_db.Add(skill);
				_db.SaveChanges();
				return true;
			}
			return false;
		}
		public List<Skill> ReadAll()
		{
			return _db.Skills.ToList();
		}
		public Skill? ReadById(int id)
		{
			return _db.Skills.FirstOrDefault(s => s.Id == id);
		}
		/// <summary>
		/// Return false if Skill doesn't exist, true if updated
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newSkillData"></param>
		public bool Update(Skill newSkillData)
		{
			var Skill = ReadById(newSkillData.Id);
			if (Skill != null)
			{
				Skill.Name = newSkillData.Name;
				_db.SaveChanges();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Return false if Skill doesn't exist, true if deleted
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newSkillData"></param>
		public bool Delete(int id)
		{
			var Skill = ReadById(id);
			if (Skill != null)
			{
				_db.Remove(Skill);
				_db.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
