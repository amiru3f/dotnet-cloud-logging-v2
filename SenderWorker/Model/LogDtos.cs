namespace SenderWorker;

public record Person(string Name, string Lastname, string Job);
public record Grade(string Name, int Number);
public record WorkerLogContainer(Person Person, Grade Grade);