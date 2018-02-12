public class AnalyticsGem {

	public string selection, action, input, time;

	public AnalyticsGem(string selection, string action, string input, float actionTime)
	{
        this.selection = selection;
		this.action = action;
		this.input = input;
        time = actionTime.ToString();
	}
}
