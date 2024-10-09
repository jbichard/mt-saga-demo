namespace MTDemo.JobConsumer.SurveyImport
{
	public class Question(string id)
	{
		public string QuestionId { get; } = id;
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
			await Task.Delay(2000);
			var survey = new Survey();
			for (int i = 0; i < 5; i++)
			{
				survey.Questions.Add(new Question(i.ToString()));
			}
			return survey;
		}
	}
}
