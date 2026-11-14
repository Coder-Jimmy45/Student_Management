namespace Student_Management.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);
