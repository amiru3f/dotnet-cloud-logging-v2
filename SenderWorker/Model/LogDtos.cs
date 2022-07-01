namespace SenderWorker;

public record Person(string Name, string Lastname, string Job);
public record Grade(string Name, int Number);

public class WorkerLogContainer
{
    public Person Person { set; get; }
    public Grade Grade { set; get; }
}