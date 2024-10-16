namespace MTDemo.Sagas.SurveyImport
{
	public class Question()
	{
		public string QuestionId { get; set; }
	}

	public class Survey
	{
		public string SurveyId { get; set; } = Guid.NewGuid().ToString();
		public List<Question> Questions { get; set; } = [];
	}

	public class Mocks
	{
		public async static Task<Survey> GetSurvey(Guid surveyImportId)
		{
			Console.WriteLine($"Fetch survey data");
			await Task.Delay(1000);
			var survey = new Survey();
			for (int i = 0; i < 5; i++)
			{
				survey.Questions.Add(new Question() { QuestionId = i.ToString() });
			}
			return survey;
		}
	}
}
