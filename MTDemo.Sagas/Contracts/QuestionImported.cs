namespace MTDemo.Sagas.Contracts
{
	public class QuestionImported
	{
		public string QuestionId { get; set; }
		public bool Result { get; set; }
		public string Error { get; set; }
	}
}
